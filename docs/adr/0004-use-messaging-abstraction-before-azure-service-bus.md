# ADR-0004: Use Messaging Abstraction Before Azure Service Bus

## Status

Accepted

## Context

Claim submission should emit an integration event so downstream workflows such as notifications can react asynchronously. The target architecture uses Azure Service Bus, but Release 2 should avoid real Azure resources and secrets.

## Decision

Add a small messaging abstraction in `EnterpriseClaims.BuildingBlocks` and use an in-memory publisher for local execution.

Claims.Api publishes `ClaimSubmittedEvent` after a valid claim is persisted. Notification.Worker consumes a seeded fake/local event for now to demonstrate the worker boundary without requiring cross-process durable messaging.

Future releases will map this abstraction to Azure Service Bus topics or queues.

## Consequences

- Service code depends on a stable application-facing abstraction rather than Azure SDK types.
- Local development remains simple.
- Notification.Worker exists as a real process boundary, even though durable delivery is deferred.
- A future Azure Service Bus implementation must define retry, dead-letter, idempotency, and observability behavior.

## Alternatives Considered

- Call Notification.Worker synchronously from Claims.Api: simpler, but couples service boundaries.
- Add Azure Service Bus immediately: production-like, but adds cloud setup and secret management too early.
- Use only logs for events: easy, but does not create a useful messaging path for later releases.
