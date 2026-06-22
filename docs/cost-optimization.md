# Cost Optimization Strategy

This document outlines the cost strategy for the Enterprise Claims Processing Platform.

## Development vs. Production SKUs

Our Bicep Infrastructure as Code (IaC) is explicitly parameterized to utilize the most cost-effective tiers available during local development or staging environments, ensuring minimal cloud spend while testing.

For a true Production workload, the infrastructure must be manually scaled to handle SLA guarantees and high traffic.

| Resource | Dev/Test SKU | Production SKU | Rationale for Production |
| -------- | ------------ | -------------- | ------------------------ |
| **Azure Container Apps** | Consumption | Dedicated Workload Profiles | Predictable pricing for high sustained traffic and dedicated hardware. |
| **Azure SQL Database** | Basic | Standard (S3+) or Premium | Guaranteed compute resources, faster I/O, and geo-replication capabilities. |
| **Azure Service Bus** | Basic | Standard / Premium | Standard supports topics/subscriptions. Premium is required for VNet integration. |
| **Azure Container Registry**| Basic | Premium | Premium allows Private Link endpoints for secure image pulls. |
| **Azure Key Vault** | Standard | Standard / Premium | Premium is only needed if HSM-backed keys are required. |

## Scaling to Zero

By leveraging **Azure Container Apps (Consumption profile)**, background workers like the `Notification.Worker` and potentially the APIs themselves can literally scale to zero replicas when there is no incoming traffic or messages in the queue. KEDA (Kubernetes Event-driven Autoscaling) monitors the Service Bus queue and spins up instances instantly when claims arrive.

## Log Retention

Log Analytics can become a significant hidden cost.
- **Dev/Test**: Configure retention to the minimum 30 days.
- **Production**: Route long-term audit logs to cheap Azure Blob Storage, keeping only the last 30-90 days in Log Analytics for active querying.
