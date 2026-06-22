# Prompt 06 — Release 6 Azure DevOps Pipelines

```text
Read AGENTS.md, docs/codex/devops-rules.md, and use the enterprise-claims-solution-architecture skill.

Implement Release 6 Azure DevOps CI/CD pipeline structure.

Scope:
1. Create pipelines/azure-pipelines.yml.
2. Create reusable templates:
   - build-dotnet.yml
   - test-dotnet.yml
   - docker-build.yml
   - security-scan.yml
   - deploy-infra-bicep.yml
   - deploy-container-apps.yml
   - smoke-test.yml
3. Use placeholders for service connections, registry names, and environments.
4. Add comments explaining what must be configured in Azure DevOps.
5. Update docs/ci-cd-strategy.md.
6. Add ADR for the CI/CD strategy.

Constraints:
- Do not hard-code secrets.
- Do not assume a real Azure DevOps organization name.
- Keep the pipeline readable for portfolio review.

Validate YAML structure as much as possible locally and summarize remaining manual configuration.
```
