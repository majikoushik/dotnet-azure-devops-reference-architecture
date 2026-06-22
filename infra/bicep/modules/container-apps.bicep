param location string = resourceGroup().location
param environmentName string
param logAnalyticsWorkspaceId string
param logAnalyticsCustomerId string
param tags object
param acrLoginServer string
param identityId string
param identityClientId string
param environmentSuffix string

resource containerAppEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: environmentName
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsCustomerId
        sharedKey: listKeys(logAnalyticsWorkspaceId, '2022-10-01').primarySharedKey
      }
    }
  }
}

var apps = [
  'api-gateway'
  'claims-api'
  'customer-api'
  'notification-worker'
]

// Note: This provisions the apps using a placeholder public helloworld image
// to successfully bootstrap the infrastructure. The actual application code
// is deployed via the CD pipeline overwriting these images.
resource containerApps 'Microsoft.App/containerApps@2024-03-01' = [for app in apps: {
  name: 'ca-${app}-${environmentSuffix}'
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identityId}': {}
    }
  }
  properties: {
    managedEnvironmentId: containerAppEnv.id
    configuration: {
      ingress: {
        external: (app == 'api-gateway' || app == 'claims-api' || app == 'customer-api')
        targetPort: 8080
      }
      registries: [
        {
          server: acrLoginServer
          identity: identityId
        }
      ]
    }
    template: {
      containers: [
        {
          name: app
          image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Development'
            }
          ]
        }
      ]
    }
  }
}]

output environmentId string = containerAppEnv.id
output defaultDomain string = containerAppEnv.properties.defaultDomain
