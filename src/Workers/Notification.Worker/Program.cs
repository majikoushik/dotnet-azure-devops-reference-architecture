using EnterpriseClaims.BuildingBlocks.Observability;
using Notification.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddEnterpriseObservability();

builder.Services.Configure<LocalNotificationOptions>(
    builder.Configuration.GetSection(LocalNotificationOptions.SectionName));

builder.Services.AddSingleton<ClaimSubmittedNotificationHandler>();
builder.Services.AddHostedService<LocalClaimSubmittedEventConsumer>();

var host = builder.Build();
host.Run();
