# Architecture Overview

## Purpose

The Enterprise Claims Processing Platform models a realistic insurance claims workflow while staying small enough for portfolio review. It demonstrates service boundaries, cloud-native .NET implementation patterns, Azure design, DevOps automation, infrastructure as code, security, and observability.

## Business Workflow

```mermaid
flowchart TD
    A[Customer registers or updates profile] --> B[Policy eligibility is checked]
    B --> C[Customer submits claim]
    C --> D[Claim documents are uploaded]
    D --> E[Claim is validated]
    E --> F[Risk score is calculated]
    F --> G[Claim status is updated]
    G --> H[Notification is sent]
    G --> I[Operational reporting is updated]
```

## Service Boundaries

| Service | Responsibility |
| --- | --- |
| ApiGateway | External API entry point, YARP routing, auth boundary, request correlation |
| Customer.Api | Customer/member profile and contact information |
| Claims.Api | Claim submission, claim lifecycle, document metadata, status changes |
| Eligibility.Api | Policy lookup and eligibility validation |
| RiskScoring.Api | Rules-based risk scoring for submitted claims |
| Reporting.Api | Read-only operational reporting APIs |
| Notification.Worker | Async notification processing from claim events |
| Shared/BuildingBlocks | Cross-cutting primitives such as errors, messaging, storage abstractions |
| Shared/Contracts | DTOs and integration contracts shared at service boundaries |
| Shared/Observability | Correlation, logging, tracing, and health-check helpers |

## Logical Architecture

```mermaid
flowchart LR
    subgraph Edge
        Client[Client Applications]
        Gateway[YARP ApiGateway]
    end

    subgraph Services
        Customer[Customer.Api]
        Claims[Claims.Api]
        Eligibility[Eligibility.Api]
        Risk[RiskScoring.Api]
        Reporting[Reporting.Api]
        Notification[Notification.Worker]
    end

    subgraph Platform
        Sql[(SQL Server / Azure SQL)]
        Blob[(Blob Storage)]
        Bus[(Service Bus)]
        Monitor[OpenTelemetry / App Insights]
    end

    Client --> Gateway
    Gateway --> Customer
    Gateway --> Claims
    Gateway --> Eligibility
    Gateway --> Risk
    Gateway --> Reporting
    Claims --> Blob
    Claims --> Bus
    Bus --> Notification
    Customer --> Sql
    Claims --> Sql
    Eligibility --> Sql
    Risk --> Sql
    Reporting --> Sql
    Services --> Monitor
```

## Current Release 1 Implementation

```mermaid
flowchart LR
    Client[Client] --> Gateway[ApiGateway]
    Gateway --> Customer[Customer.Api]
    Gateway --> Claims[Claims.Api]
    Claims --> Contracts[EnterpriseClaims.Contracts]
    Customer --> Contracts
    Claims --> BuildingBlocks[EnterpriseClaims.BuildingBlocks]
    Customer --> BuildingBlocks
    Tests[EnterpriseClaims.UnitTests] --> Claims
```

Release 1 includes only the executable foundation:

- `ApiGateway` uses YARP to route `/customers/{**catch-all}` to Customer.Api and `/claims/{**catch-all}` to Claims.Api.
- `Customer.Api` exposes a health endpoint and one fictional sample customer read endpoint.
- `Claims.Api` exposes a health endpoint and a basic claim submission endpoint.
- `EnterpriseClaims.Contracts` contains DTOs and an initial claim-submitted event contract.
- `EnterpriseClaims.BuildingBlocks` contains common response, error, and validation primitives.
- `EnterpriseClaims.UnitTests` protects the first non-trivial claim validation behavior.
- `docker-compose.yml` starts the gateway and two APIs for local development.

## Architecture Principles

- Keep controllers or endpoints thin; business rules belong in application/domain services.
- Use DTOs and contracts at service boundaries; do not expose EF entities directly.
- Prefer async APIs and cancellation tokens for request-handling and I/O.
- Keep cross-cutting concerns reusable and centralized.
- Avoid circular dependencies between services.
- Use local fakes or in-memory implementations where Azure resources are not required for development.
- Capture important trade-offs in ADRs.

## Release 1 Scope

Release 1 creates the solution skeleton, initial service projects, shared contracts/building blocks, tests, and a local validation path. It intentionally does not add persistence, real authentication, Azure infrastructure, Service Bus, Blob Storage, or production observability wiring.
