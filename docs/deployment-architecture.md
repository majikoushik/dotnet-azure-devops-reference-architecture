# Deployment Architecture

This document describes the Azure infrastructure required to deploy the Enterprise Claims Processing Platform.

## Infrastructure as Code (IaC)

We use **Azure Bicep** to provision all infrastructure. The Bicep templates are organized modularly in the `infra/bicep` directory.

### Core Resources
- **Azure Container Apps Environment**: Hosts the API Gateway, APIs, and background workers.
- **Azure Container Registry**: Stores the Docker images for the services.
- **Azure SQL Database**: Stores relational data (Claims).
- **Azure Service Bus**: Handles asynchronous messaging (e.g., Claim updates).
- **Azure Storage Account**: Stores binary claim document blobs.
- **Azure Key Vault**: Manages secrets securely using Managed Identity access.
- **Application Insights & Log Analytics**: Centralized observability.

## Deployment Instructions (Manual Validation)

To validate the Bicep templates locally before running them in a CI/CD pipeline, ensure you have the Azure CLI installed with Bicep support.

### 1. Validate the Template
This command compiles the Bicep files to verify syntax and basic semantics without connecting to Azure:
```bash
az bicep build --file infra/bicep/main.bicep
```

### 2. What-If Deployment (Dry Run)
To see exactly what Azure will create, modify, or delete in a target resource group, you can run a `what-if` deployment:
```bash
# Create the resource group first
az group create --name rg-entclaims-dev --location eastus

# Run the What-If
az deployment group what-if \
  --resource-group rg-entclaims-dev \
  --template-file infra/bicep/main.bicep \
  --parameters infra/bicep/parameters/dev.bicepparam
```

### 3. Actual Deployment
*(Note: Automated deployments should be run via Azure DevOps pipelines. This command is for manual execution.)*
```bash
az deployment group create \
  --resource-group rg-entclaims-dev \
  --template-file infra/bicep/main.bicep \
  --parameters infra/bicep/parameters/dev.bicepparam
```

> **Constraints**: Do NOT deploy real secrets in the `dev.bicepparam` file. Placeholders are injected via CI/CD variables natively.
