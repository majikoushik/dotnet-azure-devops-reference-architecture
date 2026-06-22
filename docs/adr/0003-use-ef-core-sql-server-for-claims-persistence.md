# ADR-0003: Use EF Core and SQL Server for Claims Persistence

## Status

Accepted

## Context

Claims.Api needs a persistence model for submitted claims. The target architecture calls for SQL Server / Azure SQL through EF Core, but Release 2 must remain easy to run locally and must not require paid Azure resources or committed secrets.

## Decision

Use EF Core for Claims.Api persistence with SQL Server as the intended provider for deployed environments.

Release 2 includes:

- `ClaimRecord` entity.
- `ClaimsDbContext`.
- Repository abstraction and EF implementation.
- Initial SQL Server migration source files.
- EF Core InMemory fallback when no `ConnectionStrings:ClaimsDatabase` value is configured.

Developers can supply a local SQL Server connection string through environment variables, user secrets, or another non-committed configuration source when they want to exercise migrations against SQL Server.

## Consequences

- The persistence boundary is production-aligned without forcing a real database for every local run.
- Claims.Api can evolve toward Azure SQL cleanly in later releases.
- EF entities remain internal to the service and are not exposed as API contracts.
- The InMemory fallback is convenient but does not replace SQL Server integration testing.

## Alternatives Considered

- Pure in-memory repository only: simpler, but too weak for the data architecture story.
- SQLite for local development: useful, but less aligned with the target SQL Server provider.
- Direct ADO.NET: more control, but unnecessary complexity for this reference architecture.
