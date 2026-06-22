using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.Contracts.Claims;

namespace Notification.Worker;

public sealed class ClaimSubmittedNotificationHandler(ILogger<ClaimSubmittedNotificationHandler> logger)
    : IMessageHandler<ClaimSubmittedEvent>
{
    public Task HandleAsync(ClaimSubmittedEvent message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation(
            "Prepared local notification for submitted claim {ClaimNumber} and customer {CustomerId}",
            message.ClaimNumber,
            message.CustomerId);

        return Task.CompletedTask;
    }
}
