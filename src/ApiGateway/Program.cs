var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
    service = "ApiGateway",
    description = "YARP gateway for the Enterprise Claims Processing Platform",
    downstreamRoutes = new[] { "/customers/{customerId}", "/claims" }
}));

app.MapReverseProxy();

app.Run();
