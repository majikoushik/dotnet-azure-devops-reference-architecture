# ADR-0007: Use Azure DevOps YAML Pipelines for CI/CD

Date: 2026-06-22

## Status

Accepted

## Context

The Enterprise Claims Processing Platform requires an automated build, test, and deployment process (CI/CD). We need to choose a tool that integrates natively with Azure, supports enterprise security requirements, allows template reuse, and supports Infrastructure as Code (IaC) lifecycle management.

## Decision

We will use **Azure DevOps (YAML Pipelines)** as the primary CI/CD orchestration engine.

## Rationale

- **First-party Azure Integration**: Native service connections (`AzureResourceManagerTemplateDeployment@3`, `AzureCLI@2`) allow zero-friction authentication to deploy Bicep and Azure Container Apps using Workload Identity Federation.
- **YAML over Classic**: Using YAML keeps pipeline definitions in source control alongside the application code, ensuring version parity and easy code reviews.
- **Enterprise Templating**: Azure DevOps pipelines support `template:` references, allowing us to build a library of highly reusable, standardized steps (e.g., `build-dotnet.yml`, `security-scan.yml`) that multiple services can share.
- **Security & Gates**: Azure DevOps provides robust Environment approval gates which are critical for enterprise deployments targeting staging or production.

## Consequences

- The repository is tightly coupled to Azure DevOps format `azure-pipelines.yml`, making migration to GitHub Actions or GitLab CI non-trivial (though the underlying scripts and Bicep remain portable).
- Pipeline validation requires an active Azure DevOps organization to run correctly.
