using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.Contracts.Customers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "Customer.Api",
    description = "Customer profile boundary for the Enterprise Claims Processing Platform"
}));

app.MapGet("/customers/{customerId}", (string customerId) =>
{
    if (!string.Equals(customerId, SampleCustomer.CustomerId, StringComparison.OrdinalIgnoreCase))
    {
        return Results.NotFound(ApiError.NotFound("customer.not_found", "The requested customer was not found."));
    }

    return Results.Ok(ApiResponse.Success(SampleCustomer));
})
.WithName("GetCustomerById");

app.Run();

static partial class Program
{
    private static readonly CustomerSummaryResponse SampleCustomer = new(
        "CUST-1001",
        "Avery Johnson",
        "avery.johnson@example.test",
        "Active");
}
