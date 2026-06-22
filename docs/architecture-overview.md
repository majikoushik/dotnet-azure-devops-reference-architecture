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

## Observability

The platform leverages **OpenTelemetry** to export standard logs, metrics, and traces:
- A shared `CorrelationIdMiddleware` ensures a consistent `X-Correlation-ID` flows through logs.
- Azure Application Insights receives this telemetry in production.
- Health checks are implemented at `/health`.

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

## Current Release 2 Implementation

```mermaid
flowchart LR
    Client[Client] --> Gateway[ApiGateway]
    Gateway --> Claims[Claims.Api]
    Claims --> DbContext[ClaimsDbContext]
    DbContext --> LocalStore[(EF Core InMemory by default)]
    DbContext -. configured connection string .-> Sql[(SQL Server)]
    Claims --> Publisher[IMessagePublisher]
    Publisher --> LocalBus[InMemoryMessageBus]
    Claims --> Event[ClaimSubmittedEvent]
    Event -. future mapping .-> ServiceBus[(Azure Service Bus)]
    Worker[Notification.Worker] --> FakeEvent[Seeded fake ClaimSubmittedEvent]
    Worker --> Notification[Local notification handler]
```

Release 2 adds:

- EF Core persistence setup in `Claims.Api`.
- A `ClaimRecord` entity and `ClaimsDbContext`.
- SQL Server migration source files for initial claim persistence.
- EF Core InMemory as the default local store when no connection string is configured.
- Shared messaging abstractions in `EnterpriseClaims.BuildingBlocks`.
- Claim submission application service that validates, persists, and publishes `ClaimSubmittedEvent`.
- `Notification.Worker` with a fake/local event consumer for the notification boundary.

Azure SQL and Azure Service Bus are design targets, not required local dependencies in this release.

## System Context

```mermaid
C4Context
    title System Context diagram for Enterprise Claims Platform

    Person(customer, "Policy Holder", "A customer of the insurance company.")
    Person(adjuster, "Claims Adjuster", "An employee handling insurance claims.")

    System(claimsPlatform, "Enterprise Claims Platform", "Allows customers to submit claims, and adjusters to validate them.")
    System_Ext(emailSystem, "Email System", "Internal Microsoft Exchange System.")

    Rel(customer, claimsPlatform, "Submits and tracks claims")
    Rel(adjuster, claimsPlatform, "Validates and processes claims")
    Rel(claimsPlatform, emailSystem, "Sends notifications using")
    Rel(emailSystem, customer, "Sends emails to")
```

## Architecture Style

The platform follows a modular microservice architecture. Services are logically separated by domain boundaries, enabling independent scaling and deployment.

## Container Diagram

```mermaid
C4Container
    title Container diagram for Enterprise Claims Platform

    Person(user, "User", "Policy Holder or Adjuster")

    System_Boundary(c1, "Enterprise Claims Platform") {
        Container(apiGateway, "API Gateway", "YARP", "Routes incoming requests to the appropriate backend service")

        Container(customerApi, "Customer API", ".NET 10", "Manages policyholder data and verification")
        Container(claimsApi, "Claims API", ".NET 10", "Core domain logic for claim submission and validation")
        Container(notificationWorker, "Notification Worker", ".NET 10 Background Service", "Listens for claim events and dispatches alerts")

        ContainerDb(sqlDb, "Claims SQL Database", "Azure SQL", "Stores relational claim data")
        ContainerQueue(serviceBus, "Message Broker", "Azure Service Bus", "Pub/Sub for domain events")
    }

    Rel(user, apiGateway, "Makes API calls to", "JSON/HTTPS")
    Rel(apiGateway, customerApi, "Routes customer traffic to", "HTTPS")
    Rel(apiGateway, claimsApi, "Routes claims traffic to", "HTTPS")

    Rel(claimsApi, sqlDb, "Reads/Writes", "EF Core / TCP")
    Rel(claimsApi, serviceBus, "Publishes ClaimSubmitted Event", "AMQP")

    Rel(notificationWorker, serviceBus, "Subscribes to ClaimSubmitted Event", "AMQP")
```

## Architecture Principles

- Keep controllers or endpoints thin; business rules belong in application/domain services.
- Use DTOs and contracts at service boundaries; do not expose EF entities directly.
- Prefer async APIs and cancellation tokens for request-handling and I/O.
- Keep cross-cutting concerns reusable and centralized.
- Avoid circular dependencies between services.
- Use local fakes or in-memory implementations where Azure resources are not required for development.
- Capture important trade-offs in ADRs.
