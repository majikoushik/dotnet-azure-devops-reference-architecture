using EnterpriseClaims.BuildingBlocks.Observability;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace EnterpriseClaims.UnitTests.Observability;

public class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenCorrelationIdMissing_GeneratesNewId()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var logger = new NullLogger<CorrelationIdMiddleware>();
        var middleware = new CorrelationIdMiddleware(async innerContext => 
        {
            await innerContext.Response.StartAsync();
        }, logger);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("X-Correlation-ID"));
        Assert.NotNull(context.Response.Headers["X-Correlation-ID"].ToString());
    }

    [Fact]
    public async Task InvokeAsync_WhenCorrelationIdPresent_PreservesId()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var existingId = "custom-correlation-123";
        context.Request.Headers["X-Correlation-ID"] = existingId;
        
        var logger = new NullLogger<CorrelationIdMiddleware>();
        var middleware = new CorrelationIdMiddleware(async innerContext => 
        {
            await innerContext.Response.StartAsync();
        }, logger);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("X-Correlation-ID"));
        Assert.Equal(existingId, context.Response.Headers["X-Correlation-ID"].ToString());
    }
}
