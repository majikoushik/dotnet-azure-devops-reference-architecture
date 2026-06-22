# Prompt 05 — Release 5 Bicep Infrastructure

```text
Read AGENTS.md, docs/codex/devops-rules.md, and use the enterprise-claims-solution-architecture skill.

Implement Release 5 Azure infrastructure as code using Bicep.

Scope:
1. Create infra/bicep/main.bicep.
2. Create reusable modules for:
   - Container Apps environment
   - Container Registry
   - Azure SQL logical server/database using safe placeholders
   - Service Bus namespace/queue/topic as appropriate
   - Storage Account
   - Key Vault
   - Log Analytics Workspace
   - Application Insights
3. Add dev parameter file with placeholder values only.
4. Add deployment notes to docs/deployment-architecture.md.
5. Add ADR for Azure Container Apps and Bicep.

Constraints:
- No real subscription IDs, tenant IDs, secrets, or personal values.
- Use cost-conscious SKUs for dev.
- Do not run `az deployment` unless explicitly approved.

Run static validation commands only if available locally. Summarize validation and manual deployment command examples.
```
