# Prompt 02 — Release 2 Data and Messaging

```text
Read AGENTS.md and use the enterprise-claims-solution-architecture skill.

Implement Release 2: data persistence and messaging abstraction.

Scope:
1. Add EF Core persistence for Claims.Api using SQL Server provider.
2. Add initial claim entity and DbContext.
3. Add migration setup, but do not require a real cloud database.
4. Add a messaging abstraction in Shared/BuildingBlocks.
5. Add ClaimSubmitted event contract.
6. Add Notification.Worker that can consume a fake/local notification request for now.
7. Document future Azure Service Bus mapping.
8. Add or update tests for claim submission behavior.
9. Update docs/architecture-overview.md and docs/adr with messaging and persistence decisions.

Constraints:
- Keep local development simple.
- No real Azure resources yet.
- No secrets in files.

Run relevant dotnet commands and summarize validation.
```
