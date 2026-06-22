# Prompt 03 — Release 3 Security

```text
Read AGENTS.md, docs/codex/security-rules.md, and use the enterprise-claims-solution-architecture skill.

Implement Release 3 security foundation.

Scope:
1. Add JWT authentication configuration using placeholder/local settings only.
2. Add role-based authorization policies for Customer, ClaimProcessor, Supervisor, and Admin.
3. Protect claim submission and claim status endpoints appropriately.
4. Add safe configuration examples using appsettings.Development.json placeholders or .env.example.
5. Add documentation for production security using Microsoft Entra ID, Managed Identity, and Key Vault.
6. Add tests for authorization behavior where practical.
7. Update docs/security-architecture.md and add ADR for the authentication approach.

Constraints:
- Never commit real secrets.
- Do not log tokens or sensitive claim data.
- Keep the implementation demo-friendly but production-aware.

Run relevant tests and summarize validation.
```
