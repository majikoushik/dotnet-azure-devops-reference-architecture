namespace Notification.Worker;

public sealed class LocalNotificationOptions
{
    public const string SectionName = "LocalNotifications";

    public bool SeedFakeClaimSubmittedEvent { get; init; } = true;
}
