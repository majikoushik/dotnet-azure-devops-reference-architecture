using EnterpriseClaims.Contracts.Claims;
using Microsoft.Extensions.Options;

namespace Notification.Worker;

public sealed class LocalClaimSubmittedEventConsumer(
    ClaimSubmittedNotificationHandler handler,
    IOptions<LocalNotificationOptions> options,
    ILogger<LocalClaimSubmittedEventConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Value.SeedFakeClaimSubmittedEvent)
        {
            logger.LogInformation("Local fake claim notification consumption is disabled.");
            return;
        }

        var fakeEvent = new ClaimSubmittedEvent(
            "CLM-LOCAL-0001",
            "CUST-1001",
            "POL-2026-0001",
            1250.50m,
            DateTimeOffset.UtcNow);

        await handler.HandleAsync(fakeEvent, stoppingToken);
    }
}
