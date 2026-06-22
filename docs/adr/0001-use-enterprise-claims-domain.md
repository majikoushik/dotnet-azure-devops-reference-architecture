# ADR-0001: Use Enterprise Claims Domain

## Status

Accepted

## Context

The repository needs a realistic business domain that can demonstrate solution architecture, service boundaries, security, observability, DevOps, and Azure infrastructure without becoming too large for portfolio review.

Insurance claims processing provides a strong enterprise scenario because it includes customer data, policy eligibility, document handling, workflow status, risk evaluation, async notification, reporting, compliance concerns, and operational monitoring.

## Decision

Use an Enterprise Claims Processing Platform as the primary domain for this reference architecture.

The platform will model:

- Customer/member profile management.
- Claim submission and lifecycle tracking.
- Policy eligibility checks.
- Claim document handling.
- Rules-based risk scoring.
- Async notifications.
- Operational reporting.

The implementation will stay intentionally understandable. Complex insurance logic, advanced AI scoring, and production integrations can be documented as future extensions rather than implemented prematurely.

## Consequences

- The architecture can demonstrate realistic service boundaries and cross-cutting concerns.
- Security and observability decisions have credible business context.
- Azure services such as SQL, Storage, Service Bus, Key Vault, Container Apps, and Application Insights fit naturally.
- The repository remains suitable for incremental release-by-release delivery.

## Alternatives Considered

- Generic task management system: easier to implement, but too weak for enterprise architecture storytelling.
- E-commerce platform: familiar, but less aligned with claims-specific document, eligibility, and risk workflows.
- Healthcare records platform: realistic, but likely to distract with heavier regulatory complexity.
