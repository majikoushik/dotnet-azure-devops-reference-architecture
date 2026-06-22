---
name: enterprise-claims-solution-architecture
description: Use for designing, implementing, reviewing, or documenting the Enterprise Claims Processing Platform, especially .NET services, Azure architecture, Azure DevOps pipelines, Bicep IaC, security, observability, ADRs, and release planning.
---

# Enterprise Claims Solution Architecture Skill

## Purpose

Use this skill whenever a task affects architecture, service boundaries, Azure resources, CI/CD, security, observability, or documentation for the Enterprise Claims Processing Platform.

This skill should help Codex behave like a senior .NET cloud engineer working under a solution architect.

---

## Business Domain

The platform represents a realistic insurance claims workflow:

1. Customer/member registration.
2. Policy lookup or eligibility check.
3. Claim submission.
4. Claim document upload.
5. Claim validation.
6. Risk scoring.
7. Claim status updates.
8. Async notification.
9. Operational reporting.

Keep the domain understandable. Do not overcomplicate the insurance logic unless the prompt specifically asks for it.

---

## Architecture Principles

Follow these principles:

- Design for clear service boundaries.
- Use API contracts intentionally.
- Keep cross-cutting concerns in shared building blocks.
- Favor simple production-like patterns over toy examples.
- Keep architecture documentation current.
- Capture significant trade-offs in ADRs.
- Do not introduce cloud resources without explaining cost and operational implications.
- Prefer secure-by-default design.
- Prefer observable-by-default design.

---

## Standard Workflow

For every task:

1. Read `AGENTS.md`.
2. Identify the architectural area affected:
   - API/service design
   - persistence
   - messaging
   - security
   - observability
   - DevOps
   - infrastructure
   - documentation
3. Create a short plan.
4. Implement a small, reviewable change.
5. Add or update tests.
6. Update docs or ADRs when applicable.
7. Run validation commands.
8. Summarize output using the repository Definition of Done.

---

## Service Design Guidance

Preferred services:

- `Customer.Api`: customer/member profile and contact information.
- `Claims.Api`: claim submission, claim status, claim lifecycle.
- `Eligibility.Api`: policy and eligibility validation.
- `RiskScoring.Api`: rules-based risk score; future AI-assisted scoring may be documented but not required initially.
- `Reporting.Api`: read-only reporting and operational dashboard APIs.
- `Notification.Worker`: async notification consumer.
- `ApiGateway`: YARP-based routing, security boundary, and API entry point.

Avoid circular dependencies between services.

---

## .NET Implementation Guidance

- Use ASP.NET Core minimal APIs or controllers consistently; do not mix styles randomly inside the same service.
- Use EF Core for SQL persistence.
- Keep migration naming descriptive.
- Use options pattern for configuration.
- Use health checks for each service.
- Use structured logging.
- Add correlation ID middleware once and reuse it.
- Use Problem Details for consistent API errors.
- Keep sample data realistic but fictional.

---

## Security Guidance

When security is involved:

- Use JWT bearer authentication for app-level demo security.
- Use role-based authorization for claim processor, supervisor, admin, and customer scenarios.
- Use Azure Managed Identity in production design.
- Use Azure Key Vault for production secret references.
- Do not put secrets into `appsettings.json`, pipeline YAML, Bicep parameter files, README examples, or test data.
- Do not log PII, claim document content, tokens, passwords, or connection strings.

---

## Azure DevOps Guidance

When pipelines are involved:

- Prefer reusable templates under `pipelines/templates/`.
- CI should include restore, build, test, coverage, static analysis placeholder, Docker build, and artifact publish.
- CD should include infrastructure deployment, app deployment, smoke tests, and health checks.
- Use variable groups or secure pipeline variables for secrets, but never hard-code values.
- Use clear stages: `Validate`, `Build`, `Test`, `Package`, `DeployInfra`, `DeployApps`, `SmokeTest`.
- Add comments explaining portfolio-relevant decisions.

---

## Bicep/IaC Guidance

When creating Azure infrastructure:

- Use Bicep modules.
- Include parameter files for `dev` first.
- Use deterministic resource naming with environment suffixes.
- Include tags such as `project`, `environment`, `owner`, and `costCenter` with placeholder values.
- Prefer Azure Container Apps, Azure Container Registry, Azure SQL, Service Bus, Key Vault, Storage Account, Log Analytics, Application Insights, and optional Redis.
- Do not create expensive SKUs by default.
- Use comments to explain production alternatives.

---

## Observability Guidance

Include:

- Correlation IDs.
- Structured logs.
- Health checks.
- OpenTelemetry traces.
- Metrics where useful.
- Application Insights integration design.
- Example KQL queries in `docs/observability.md` when Application Insights is added.

---

## Documentation Guidance

For architecture documentation:

- Use Mermaid diagrams where possible.
- Keep documents readable for recruiters and architects.
- Emphasize trade-offs, operational reasoning, and cloud design maturity.
- Add ADRs for decisions such as API Gateway choice, messaging choice, hosting model, identity model, observability model, and IaC tool choice.

ADR format:

```md
# ADR-000X: Decision Title

## Status
Accepted

## Context

## Decision

## Consequences

## Alternatives Considered
```

---

## Final Response Format

After completing a task, summarize:

1. What changed.
2. Files changed.
3. Tests/checks run.
4. Architecture/security/DevOps impact.
5. Remaining follow-ups.
