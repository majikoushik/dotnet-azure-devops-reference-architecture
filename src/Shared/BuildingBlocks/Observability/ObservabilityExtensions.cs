using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace EnterpriseClaims.BuildingBlocks.Observability;

public static class ObservabilityExtensions
{
    public static IHostApplicationBuilder AddEnterpriseObservability(this IHostApplicationBuilder builder)
    {
        // Add OpenTelemetry Tracing and Metrics
        var otel = builder.Services.AddOpenTelemetry();

        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
        });

        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddRuntimeInstrumentation();
        });

        // Use Application Insights if configured, otherwise fallback to local console exporter
        var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
        {
            otel.UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });
        }
        else
        {
            otel.WithTracing(tracing => tracing.AddConsoleExporter());
            otel.WithMetrics(metrics => metrics.AddConsoleExporter());
        }

        // Configure standard logging
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        return builder;
    }
}
