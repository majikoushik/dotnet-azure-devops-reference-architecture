# Prompt 01 — Release 1 Foundation

```text
Read AGENTS.md and use the enterprise-claims-solution-architecture skill.

Implement Release 1 foundation for the Enterprise Claims Processing Platform.

Scope:
1. Create the .NET solution structure.
2. Add ApiGateway using YARP Reverse Proxy.
3. Add Customer.Api with simple health endpoint and sample customer read endpoint.
4. Add Claims.Api with simple health endpoint and claim submission endpoint.
5. Add Shared/Contracts for DTOs/events.
6. Add Shared/BuildingBlocks for common API response/problem details placeholder.
7. Add basic unit tests for any non-trivial logic.
8. Add docker-compose.yml for local service startup if practical.
9. Update README and docs/architecture-overview.md.
10. Add ADR for API Gateway decision if YARP is introduced.

Constraints:
- Keep implementation small and clean.
- Do not add real authentication yet.
- Do not add Azure infrastructure yet.
- Do not use real secrets.

Run dotnet restore, build, and test if possible. Summarize files changed, validation, and next prompt.
```
