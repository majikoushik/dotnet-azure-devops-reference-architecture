# AGENTS.md

## Repository Mission

This repository is a portfolio-grade **Enterprise Claims Processing Platform for the Insurance Domain**.

The goal is to showcase two capabilities together:

1. **Solution Architect capability**: cloud-native .NET architecture, Azure design, security, reliability, observability, DevOps, IaC, and architecture documentation.
2. **Agentic AI engineering capability**: disciplined use of Codex with repo guidance, reusable skills, command rules, prompts, review loops, and documentation-first delivery.

This is not a random CRUD application. Every implementation should strengthen the reference architecture story.

---

## Target Architecture

Use this reference architecture unless a task explicitly changes it:

- Backend: ASP.NET Core Web APIs using .NET 10 LTS where available; keep .NET 8 compatibility if the local environment does not support .NET 10 yet.
- Architecture style: modular microservice-ready architecture with clear service boundaries.
- Domain: insurance claim submission, validation, eligibility, risk scoring, document handling, notification, and reporting.
- API gateway: YARP Reverse Proxy.
- Persistence: Azure SQL / SQL Server through EF Core.
- Messaging: Azure Service Bus abstraction; local development may use an in-memory or fake bus until Azure wiring exists.
- Storage: Azure Blob Storage abstraction for claim documents.
- Security: JWT authentication, role-based authorization, Managed Identity design, Key Vault for production secrets.
- Observability: structured logging, correlation IDs, OpenTelemetry, Application Insights design.
- DevOps: Azure DevOps YAML pipelines with reusable templates.
- Infrastructure as Code: Bicep-first. Terraform may be added later only if specifically requested.
- Local development: Docker Compose where useful; avoid forcing paid Azure resources for local demos.

---

## Expected Repository Shape

Prefer this structure:

```text
src/
  ApiGateway/
  Services/
    Customer.Api/
    Claims.Api/
    Eligibility.Api/
    RiskScoring.Api/
    Reporting.Api/
  Workers/
    Notification.Worker/
  Shared/
    BuildingBlocks/
    Contracts/
    Observability/

tests/
  UnitTests/
  IntegrationTests/
  ArchitectureTests/

infra/
  bicep/
    main.bicep
    modules/
    parameters/

pipelines/
  azure-pipelines.yml
  templates/

docs/
  architecture-overview.md
  security-architecture.md
  observability.md
  ci-cd-strategy.md
  deployment-architecture.md
  cost-optimization.md
  disaster-recovery.md
  adr/
```

Do not create unnecessary folders. Grow this structure release by release.

---

## Agent Working Agreement

Before changing code, Codex should:

1. Read this file.
2. Identify the impacted services, tests, docs, and pipeline/infra files.
3. Propose a short implementation plan.
4. Make small, reviewable changes.
5. Run the narrowest relevant validation first, then broader validation when needed.
6. Summarize changed files, verification results, and any residual risks.

Do not implement broad rewrites unless explicitly requested.

---

## Quality Rules

### Code

- Use clean, readable C# with explicit names.
- Keep controllers thin; put business logic in application/domain services.
- Use DTOs/contracts at service boundaries. Do not expose EF entities directly from APIs.
- Prefer async APIs for I/O.
- Use cancellation tokens for request-handling and external calls.
- Centralize error handling and return consistent API error responses.
- Avoid premature abstraction, but do not duplicate cross-cutting concerns across services.
- Do not add production dependencies without explaining why.

### Tests

- Add or update tests for meaningful behavior changes.
- Unit tests should cover domain/application logic.
- Integration tests should cover API + persistence boundaries where practical.
- Architecture tests should protect service boundaries and dependency direction.
- If a test cannot be run locally, document the reason and provide the intended command.

### Documentation

Every major feature should update at least one relevant documentation file:

- `docs/architecture-overview.md` for system-level design changes.
- `docs/adr/` for meaningful architecture decisions.
- `docs/security-architecture.md` for authentication, authorization, secrets, identity, or network changes.
- `docs/observability.md` for logging, tracing, metrics, health checks, or dashboards.
- `docs/ci-cd-strategy.md` for pipeline changes.
- `docs/deployment-architecture.md` for Azure/Bicep changes.

### Security

- Never commit real secrets, connection strings, API keys, certificates, tokens, or passwords.
- Use placeholders in examples.
- For production design, prefer Managed Identity and Key Vault.
- Validate input at API boundaries.
- Do not log sensitive user, claim, policy, or document data.
- Keep generated sample data fictional.

### DevOps and IaC

- Prefer YAML pipeline templates for reuse.
- Separate CI and CD concerns.
- CI should build, test, scan, package, and publish artifacts/images.
- CD should deploy infrastructure, deploy apps, run smoke tests, and expose health status.
- Bicep modules should be small and reusable.
- Infrastructure files must avoid hard-coded subscription IDs, tenant IDs, secrets, or personal values.

---

## Definition of Done

A Codex task is done only when the response includes:

- Summary of what changed.
- Files changed.
- Tests/checks run and results.
- Documentation updated or explanation for why not needed.
- Security/architecture impact if relevant.
- Follow-up items, if any.

---

## Useful Commands

Use these when the project structure exists:

```bash
dotnet restore
dotnet build
dotnet test
```

For local containers, use Docker Compose only after `docker-compose.yml` exists:

```bash
docker compose up --build
```

For Azure DevOps and Azure CLI commands, ask for confirmation before running anything that may change cloud resources.

---

## Prompting Style for This Repository

When a user asks for a feature, Codex should interpret it as:

> Implement the smallest production-like slice that improves the reference architecture and keeps docs, tests, and DevOps alignment intact.

If requirements are ambiguous, make a reasonable architecture-safe assumption and state it in the final summary. Do not stop unless the ambiguity would cause destructive or expensive action.

---

## Repo-Specific Skill

Use the repo skill when relevant:

- `.agents/skills/enterprise-claims-solution-architecture/SKILL.md`

Use this skill for architecture planning, service design, Azure DevOps, Bicep/IaC, security, observability, ADRs, and release prompts.
