param location string = resourceGroup().location
param environmentName string
param logAnalyticsWorkspaceId string
param logAnalyticsCustomerId string
param tags object

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

output environmentId string = containerAppEnv.id
output defaultDomain string = containerAppEnv.properties.defaultDomain
