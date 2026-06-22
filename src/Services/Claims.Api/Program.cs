using Claims.Api.Application;
using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.Contracts.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ClaimSubmissionValidator>();
builder.Services.AddSingleton<ClaimNumberGenerator>();

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
    ClaimSubmissionValidator validator,
    ClaimNumberGenerator claimNumberGenerator,
    CancellationToken cancellationToken) =>
{
    cancellationToken.ThrowIfCancellationRequested();

    var validation = validator.Validate(request);
    if (!validation.IsValid)
    {
        return Results.BadRequest(ApiResponse.Failure<ClaimSubmissionResponse>(validation.Errors));
    }

    var response = new ClaimSubmissionResponse(
        claimNumberGenerator.Create(),
        "Submitted",
        DateTimeOffset.UtcNow);

    return Results.Accepted($"/claims/{response.ClaimNumber}", ApiResponse.Success(response));
})
.WithName("SubmitClaim");

app.Run();

public partial class Program
{
}
