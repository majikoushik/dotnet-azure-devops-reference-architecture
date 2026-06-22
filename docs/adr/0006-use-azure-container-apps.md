# ADR-0006: Use Azure Container Apps for Microservices Hosting

Date: 2026-06-22

## Status

Accepted

## Context

The Enterprise Claims Processing Platform follows a modular microservice architecture. We need a reliable hosting environment in Azure that supports containers natively, handles dynamic scaling, and simplifies operations compared to managing an entire Kubernetes cluster directly.

## Decision

We will use **Azure Container Apps (ACA)** as the primary hosting environment for our microservices (e.g., ApiGateway, Claims.Api, Customer.Api, Notification.Worker).

Additionally, we will use **Bicep** as our Infrastructure as Code (IaC) tool to provision the Container Apps environment and all supporting Azure resources (SQL, Service Bus, Key Vault).

## Rationale

- **Serverless Kubernetes**: ACA provides the benefits of Kubernetes (KEDA scaling, Envoy routing, Dapr integration if needed) without the operational overhead of managing AKS node pools or upgrades.
- **Microservices Native**: Built-in support for internal/external ingress, container revisions, and seamless secrets integration.
- **Cost Efficiency**: Scale-to-zero capabilities for background workers or low-traffic environments.
- **Bicep Integration**: Bicep provides a native, declarative syntax for Azure resources, fully supported by Microsoft, avoiding third-party state file management (e.g. Terraform) for this reference architecture.

## Consequences

- The platform relies exclusively on Azure for hosting.
- We cannot use raw Kubernetes manifests natively; all configurations must translate to the ACA resource model via Bicep.
