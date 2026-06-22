using Claims.Api.Application;
using Claims.Api.Data;
using Claims.Api.Domain;
using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.BuildingBlocks.Observability;
using EnterpriseClaims.BuildingBlocks.Security;
using EnterpriseClaims.Contracts.Claims;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ClaimsDbContext>("ClaimsDbContextHealthCheck");
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ClaimSubmissionValidator>();
builder.Services.AddSingleton<ClaimNumberGenerator>();
builder.Services.AddScoped<IClaimRepository, EfClaimRepository>();
builder.Services.AddScoped<ClaimSubmissionService>();

builder.Services.AddSingleton<IMessagePublisher, InMemoryMessageBus>();
builder.Services.AddEnterpriseSecurity(builder.Configuration);

builder.AddEnterpriseObservability();

var connectionString = builder.Configuration.GetConnectionString("ClaimsDb");
builder.Services.AddDbContext<ClaimsDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        options.UseInMemoryDatabase("ClaimsDb_InMemory");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClaimsDbContext>();
    if (db.Database.IsSqlServer())
    {
        db.Database.Migrate();
    }
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Claims.Api",
    description = "Claim submission and lifecycle boundary for the Enterprise Claims Processing Platform"
}));

app.MapPost("/claims", async (
    ClaimSubmissionRequest request,
    ClaimSubmissionService service,
    CancellationToken cancellationToken) =>
{
    var result = await service.SubmitAsync(request, cancellationToken);

    if (!result.Validation.IsValid)
    {
        return Results.BadRequest(ApiResponse.Failure<ClaimSubmissionResponse>(result.Validation.Errors));
    }

    return Results.Accepted($"/claims/{result.Response!.ClaimNumber}", ApiResponse.Success(result.Response));
})
.WithName("SubmitClaim")
.RequireAuthorization("Customer");

app.MapGet("/claims/{claimNumber}", async (
    string claimNumber,
    ClaimsDbContext dbContext,
    System.Security.Claims.ClaimsPrincipal user,
    CancellationToken cancellationToken) =>
{
    var claim = await dbContext.Claims.FirstOrDefaultAsync(c => c.ClaimNumber == claimNumber, cancellationToken);

    if (claim is null)
    {
        return Results.NotFound(ApiResponse.Failure<ClaimStatusResponse>([ApiError.NotFound("claim.not_found", "Claim not found.")]));
    }

    // Authorization: Allow 'ClaimProcessor' role or verify CustomerId ownership
    var isProcessor = user.IsInRole("ClaimProcessor") || user.IsInRole("Admin");
    var customerIdClaim = user.FindFirst("customerId")?.Value;

    if (!isProcessor && (string.IsNullOrEmpty(customerIdClaim) || customerIdClaim != claim.CustomerId))
    {
        return Results.Forbid();
    }

    var response = new ClaimStatusResponse(
        claim.ClaimNumber,
        claim.CustomerId,
        claim.PolicyNumber,
        claim.EstimatedAmount,
        claim.Status,
        claim.LossDescription,
        claim.SubmittedAt);

    return Results.Ok(ApiResponse.Success(response));
})
.WithName("GetClaimStatus")
.RequireAuthorization();

app.Run();

public partial class Program
{
}
