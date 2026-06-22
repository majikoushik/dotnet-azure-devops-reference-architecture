param location string = resourceGroup().location
param appInsightsName string
param logAnalyticsWorkspaceId string
param tags object

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspaceId
  }
}

output connectionString string = appInsights.properties.ConnectionString
output instrumentKey string = appInsights.properties.InstrumentationKey
