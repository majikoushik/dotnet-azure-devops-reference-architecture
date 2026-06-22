# Enterprise Claims Processing Platform

[![Build Status](https://dev.azure.com/organization/project/_apis/build/status/claims-platform-ci?branchName=main)](https://dev.azure.com/organization/project/_build/latest?definitionId=1&branchName=main)

A portfolio-grade **Reference Architecture** demonstrating how to build a scalable, cloud-native microservices platform for the Insurance Domain using **.NET** and **Azure**.

This repository was constructed to serve as a comprehensive Solution Architect showcase, emphasizing discipline in **Enterprise Architecture, DevOps, Security, and Observability**.

## Domain: Enterprise Claims
The platform handles the core lifecycle of insurance claims:
- **Claim Submission**: Ingesting FNOL (First Notice of Loss) data.
- **Customer Management**: Maintaining policyholder records.
- **Notification**: Asynchronously alerting adjusters and customers.

*(Note: The business logic is kept intentionally concise to focus on architectural patterns rather than deep domain complexity).*

## Architecture Stack
- **API Gateway**: YARP (Yet Another Reverse Proxy)
- **Microservices**: ASP.NET Core 10 Web APIs (Claims, Customer)
- **Background Workers**: .NET Generic Hosts (Notification)
- **Persistence**: Entity Framework Core with Azure SQL Server
- **Messaging**: Azure Service Bus (Abstraction implemented, Pub/Sub Azure infrastructure planned)
- **Security**: JWT Authentication & Azure Key Vault (Managed Identities)
- **Observability**: OpenTelemetry, Application Insights, Log Analytics
- **Hosting**: Azure Container Apps (ACA)
- **IaC**: Azure Bicep

## Agentic AI Engineering Practices
This repository is unique: it was co-developed using an **Agentic AI coding assistant**.
The repository heavily enforces strict constraints via prompt engineering and agent rules to guarantee enterprise-grade output.
Key elements of this practice include:
- **`AGENTS.md`**: A strict system prompt enforcing architectural constraints, design decisions, and Definition of Done.
- **`docs/codex/`**: Specific rulebooks preventing hardcoded secrets and enforcing clean pipelines.
- **`.agents/skills/`**: Highly specialized context injected into the AI to teach it the "Enterprise Claims Solution Architecture".
- **Documentation-First Delivery**: The AI is forced to write ADRs (Architecture Decision Records) and Implementation Plans before writing a single line of code.

## Documentation
Explore the detailed architecture documentation:
- [Architecture Overview](docs/architecture-overview.md) (C4 Diagrams)
- [Security Architecture](docs/security-architecture.md)
- [Observability Strategy](docs/observability.md)
- [Deployment Architecture (IaC)](docs/deployment-architecture.md)
- [CI/CD Strategy](docs/ci-cd-strategy.md)
- [Cost Optimization](docs/cost-optimization.md)
- [Disaster Recovery](docs/disaster-recovery.md)
- [ADR Index](docs/adr/README.md)

## Roadmap & Future Enhancements
While this serves as a robust reference architecture, a true production V2 would include:
1. **Redis Caching**: Inject distributed caching via Azure Cache for Redis for high-frequency `GET` requests on the API Gateway.
2. **Private Networking**: Move Azure SQL, Service Bus, and Key Vault into a private VNet utilizing Azure Private Link.
3. **Dapr Integration**: Abstract the Service Bus and Key Vault integrations behind Dapr sidecars to further decouple the microservices from Azure SDKs.
4. **Terraform Migration**: Provide an alternative Infrastructure as Code provider utilizing Terraform alongside Bicep.

## Local Development
1. Clone the repository.
2. Run `dotnet build`.
3. Set your local environment variables or User Secrets for the SQL Connection string.
4. Run the API Gateway, Claims API, Customer API, and Notification Worker concurrently.
