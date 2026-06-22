using Claims.Api.Application;
using Claims.Api.Data;
using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.Contracts.Claims;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddClaimsPersistence(builder.Configuration);
builder.Services.AddSingleton<IMessagePublisher, InMemoryMessageBus>();
builder.Services.AddSingleton<ClaimSubmissionValidator>();
builder.Services.AddSingleton<ClaimNumberGenerator>();
builder.Services.AddScoped<ClaimSubmissionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Claims.Api",
    description = "Claim submission and lifecycle boundary for the Enterprise Claims Processing Platform"
}));

app.MapPost("/claims", (
    ClaimSubmissionRequest request,
    ClaimSubmissionService service,
    CancellationToken cancellationToken) =>
{
    return SubmitClaimAsync(request, service, cancellationToken);
})
.WithName("SubmitClaim");

app.Run();

static async Task<IResult> SubmitClaimAsync(
    ClaimSubmissionRequest request,
    ClaimSubmissionService service,
    CancellationToken cancellationToken)
{
    var result = await service.SubmitAsync(request, cancellationToken);
    if (!result.Validation.IsValid)
    {
        return Results.BadRequest(ApiResponse.Failure<ClaimSubmissionResponse>(result.Validation.Errors));
    }

    return Results.Accepted($"/claims/{result.Response!.ClaimNumber}", ApiResponse.Success(result.Response));
}

public partial class Program
{
}
