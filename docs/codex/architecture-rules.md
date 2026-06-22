# Architecture Rules

## Architectural Intent

This project is a reference architecture for an enterprise insurance claims platform. It should demonstrate architecture maturity more than feature volume.

## Core Design Rules

1. Keep service boundaries explicit.
2. Keep APIs contract-first and stable.
3. Keep shared code small and cross-cutting only.
4. Use asynchronous messaging for events that should not block a request.
5. Use SQL for transactional claim state.
6. Use Blob Storage abstraction for documents.
7. Use API Gateway as the external boundary.
8. Use health checks and telemetry in every service.
9. Use ADRs for major choices.
10. Keep local development runnable without real Azure resources.

## Main Domain Events

Start with these events:

- `ClaimSubmitted`
- `ClaimValidated`
- `EligibilityChecked`
- `RiskScoreCalculated`
- `ClaimStatusChanged`
- `NotificationRequested`

## Initial Bounded Contexts

- Customer Management
- Claims Management
- Eligibility
- Risk Scoring
- Notifications
- Reporting

## ADR Triggers

Create an ADR when changing:

- Hosting model
- Messaging approach
- Database choice
- API Gateway approach
- Authentication approach
- IaC tool
- Observability stack
- Deployment strategy
