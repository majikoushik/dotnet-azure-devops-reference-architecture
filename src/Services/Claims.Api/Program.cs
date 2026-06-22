using Claims.Api.Application;
using Claims.Api.Data;
using Claims.Api.Domain;
using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.Contracts.Claims;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ClaimSubmissionValidator>();
builder.Services.AddSingleton<ClaimNumberGenerator>();

builder.Services.AddSingleton<IMessagePublisher, InMemoryMessageBus>();

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

app.UseExceptionHandler();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Claims.Api",
    description = "Claim submission and lifecycle boundary for the Enterprise Claims Processing Platform"
}));

app.MapPost("/claims", async (
    ClaimSubmissionRequest request,
    ClaimSubmissionValidator validator,
    ClaimNumberGenerator claimNumberGenerator,
    ClaimsDbContext dbContext,
    IMessagePublisher messagePublisher,
    CancellationToken cancellationToken) =>
{
    cancellationToken.ThrowIfCancellationRequested();

    var validation = validator.Validate(request);
    if (!validation.IsValid)
    {
        return Results.BadRequest(ApiResponse.Failure<ClaimSubmissionResponse>(validation.Errors));
    }

    var claim = new ClaimRecord
    {
        Id = Guid.NewGuid(),
        ClaimNumber = claimNumberGenerator.Create(),
        CustomerId = request.CustomerId,
        PolicyNumber = request.PolicyNumber,
        EstimatedAmount = request.EstimatedAmount,
        LossDescription = request.LossDescription,
        Status = "Submitted",
        SubmittedAt = DateTimeOffset.UtcNow
    };

    dbContext.Claims.Add(claim);
    await dbContext.SaveChangesAsync(cancellationToken);

    var evt = new ClaimSubmittedEvent(
        claim.ClaimNumber,
        claim.CustomerId,
        claim.PolicyNumber,
        claim.EstimatedAmount,
        claim.SubmittedAt);

    await messagePublisher.PublishAsync(evt, cancellationToken);

    var response = new ClaimSubmissionResponse(
        claim.ClaimNumber,
        claim.Status,
        claim.SubmittedAt);

    return Results.Accepted($"/claims/{response.ClaimNumber}", ApiResponse.Success(response));
})
.WithName("SubmitClaim");

app.Run();


public partial class Program
{
}
