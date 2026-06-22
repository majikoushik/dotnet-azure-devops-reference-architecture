# ADR-0005: Use JWT and Managed Identity for Security

## Status
Accepted

## Context
The platform requires authentication to identify users (customers, claim processors) and authorization to restrict access to APIs (e.g. submitting a claim or viewing a claim status). We need a mechanism that is straightforward for local development but scalable and secure for Azure production environments.

## Decision
1. **Authentication**: We will use JWT Bearer tokens for authentication. For local development and demonstration, a simple symmetric key JWT generation process or hardcoded placeholder token approach will be used.
2. **Authorization**: We will use ASP.NET Core Role-Based Authorization Policies. We have defined four foundational roles: `Customer`, `ClaimProcessor`, `Supervisor`, and `Admin`.
3. **Production Mapping**: In a real production Azure environment, the JWT issuer will be **Microsoft Entra ID**. Downstream services will validate tokens against Entra ID. Service-to-service authentication will utilize **Azure Managed Identities** to avoid managing secrets.

## Consequences
- Developers can test the API locally by passing a simple generated JWT token without needing a full Entra ID tenant.
- API endpoints are secure by default, using standard `[Authorize(Policy = "RoleName")]` or `.RequireAuthorization("PolicyName")`.
- Transitioning to production only requires swapping the JWT issuer and audience configurations in `appsettings.json`, with zero code changes to the APIs.
