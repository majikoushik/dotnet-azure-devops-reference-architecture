# ADR-0002: Use YARP for the API Gateway

## Status

Accepted

## Context

The platform needs a single entry point that can route client traffic to independently deployable service boundaries. Release 1 only requires local routing for Customer.Api and Claims.Api, but later releases will add security, correlation, observability, and deployment concerns at the edge.

The gateway should fit naturally with ASP.NET Core, be easy to configure locally, and avoid introducing a heavier managed gateway dependency before the Azure deployment model is implemented.

## Decision

Use YARP Reverse Proxy for `ApiGateway`.

In Release 1, YARP routes:

- `/customers/{**catch-all}` to Customer.Api.
- `/claims/{**catch-all}` to Claims.Api.

Docker Compose overrides downstream destinations through environment variables so the same gateway configuration can support local process execution and containerized execution.

## Consequences

- The gateway stays in the .NET ecosystem and can share future cross-cutting middleware patterns.
- Local development remains simple and does not require Azure API Management.
- The gateway can become the central point for authentication, authorization policies, correlation, request logging, and edge-level health checks in later releases.
- YARP is a package dependency that must be restored from NuGet and watched for security updates.

## Alternatives Considered

- Direct client calls to each API: simpler initially, but weakens the reference architecture story and avoids the edge boundary.
- Azure API Management: strong production option, but too heavy and cloud-dependent for Release 1.
- NGINX or Envoy: capable reverse proxies, but less aligned with the .NET-focused architecture and local C# extensibility goals.
