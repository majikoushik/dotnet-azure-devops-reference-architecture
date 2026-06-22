param location string = resourceGroup().location
param keyVaultName string
param tenantId string
param tags object

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
    enableRbacAuthorization: true
    enabledForTemplateDeployment: true
  }
}

output keyVaultUri string = keyVault.properties.vaultUri
output keyVaultId string = keyVault.id
