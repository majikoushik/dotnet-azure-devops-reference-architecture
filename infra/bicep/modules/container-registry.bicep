param location string = resourceGroup().location
param acrName string
param tags object

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    // Disabled intentionally to strengthen Managed Identity architecture. 
    // Container apps will pull images via an AcrPull role assignment.
    adminUserEnabled: false
  }
}

output acrId string = acr.id
output acrLoginServer string = acr.properties.loginServer
