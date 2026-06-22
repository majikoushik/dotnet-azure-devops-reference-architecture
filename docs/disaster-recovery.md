# Disaster Recovery & High Availability

This document describes the architectural approach to achieving High Availability (HA) and Disaster Recovery (DR) for the Enterprise Claims Processing Platform in a production environment.

## High Availability (Intra-Region)

The current architecture natively supports intra-region HA when deployed with production-grade SKUs:
1. **Azure Container Apps**: Replicas are automatically distributed across multiple Availability Zones (AZs) by default if the selected Azure region supports them.
2. **Azure SQL Database**: Upgrading to the `Premium` or `Business Critical` tier automatically provisions Always On Availability Groups across Availability Zones.
3. **Azure Service Bus**: Upgrading to the `Premium` tier enables AZ support natively.

## Disaster Recovery (Multi-Region)

For full disaster recovery, a secondary (passive or active) region is required.

### 1. Global Traffic Routing
Inject **Azure Front Door** or **Azure Traffic Manager** globally above the API Gateway. In an active/passive setup, all traffic routes to `East US`. If that region falls offline, Front Door automatically fails over to `West US`.

### 2. Database Geo-Replication
Azure SQL supports **Active Geo-Replication** and **Auto-Failover Groups**. The primary database asynchronously replicates data to a secondary database in another region. The microservices point to a single Failover Group connection string that automatically handles the DNS switch during a failure.

### 3. Messaging
Azure Service Bus offers **Geo-Disaster Recovery (Geo-DR)**. This pairs two Service Bus namespaces (Primary and Secondary). Note that Geo-DR only replicates the *metadata* (queues, topics, subscriptions), not the actual messages in flight. Services must be resilient to potential message loss during a catastrophic regional failure.

### 4. Bicep Infrastructure
Because core infrastructure is codified in Azure Bicep (`infra/bicep/main.bicep`), standing up a fresh region's infrastructure can be heavily accelerated via the Azure DevOps CI/CD pipeline by changing the `location` variable. Note that data replication, DNS cutover, and secrets propagation would require additional runbooks or manual steps to achieve full disaster recovery.
