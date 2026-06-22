# Observability

This document outlines the observability strategy for the Enterprise Claims Processing Platform.

## Approach

The platform uses **OpenTelemetry** as the standard protocol for generating logs, metrics, and traces.

- **Local Development**: OpenTelemetry outputs traces and metrics to the console for easy debugging without requiring external dependencies.
- **Production (Azure)**: The platform uses the `Azure.Monitor.OpenTelemetry.AspNetCore` package to automatically export telemetry to **Azure Application Insights**. This is enabled simply by providing the Application Insights connection string in configuration, requiring zero code changes.

## Pillars of Observability

### 1. Tracing
Distributed tracing tracks requests as they flow across boundaries (e.g. ApiGateway -> Claims.Api -> Database/MessageBus).
All services utilize a unified `X-Correlation-ID` header. The custom `CorrelationIdMiddleware` ensures this correlation ID is preserved and attached to all logging scopes.

### 2. Logging
Structured logging is enabled by default. Since the `CorrelationIdMiddleware` attaches the correlation ID to the `ILogger` scope, every log entry automatically includes it, making cross-service querying trivial.
**Constraint**: Do NOT log sensitive claim data (PII/PHI) or secrets.

### 3. Metrics
OpenTelemetry automatically collects standard .NET metrics (CPU, Memory, Request rates).

### 4. Health Checks
ASP.NET Core Health Checks expose a `/health` endpoint on all services.
- Simple readiness checks (API is up).
- Dependency checks (e.g. `Claims.Api` checks SQL Server connectivity using `AddDbContextCheck`).

## Example KQL Queries

If analyzing logs in Azure Application Insights using Kusto Query Language (KQL), you can use the following examples:

### Find all logs for a specific request (using Correlation ID)
```kusto
traces
| where customDimensions["CorrelationId"] == "your-correlation-id"
| order by timestamp asc
```

### Identify slow requests (Latency > 2 seconds)
```kusto
requests
| where duration > 2000
| project timestamp, name, duration, resultCode, customDimensions["CorrelationId"]
| order by duration desc
```

### View claim submission failures
```kusto
traces
| where message contains "Claim submission failed"
| project timestamp, severityLevel, message, customDimensions["CorrelationId"]
```
