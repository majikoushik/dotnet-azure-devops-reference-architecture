targetScope = 'resourceGroup'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Environment short name (e.g., dev, test, prod).')
param environment string

@description('Application name prefix.')
param appName string

@description('SQL Server Admin Login')
@secure()
param sqlAdminLogin string

@description('SQL Server Admin Password')
@secure()
param sqlAdminPassword string

@description('Tenant ID for Key Vault')
param tenantId string = subscription().tenantId

@description('Resource Tags')
param tags object = {
  project: 'EnterpriseClaims'
  environment: environment
  owner: 'platform-team@example.com'
  costCenter: 'CC-1234'
}

// Resource Naming
var suffix = '${appName}-${environment}'
var uniqueSuffix = uniqueString(resourceGroup().id, environment)

var logAnalyticsName = 'law-${suffix}'
var appInsightsName = 'appi-${suffix}'
var keyVaultName = 'kv-${appName}-${environment}-${substring(uniqueSuffix, 0, 4)}'
var storageAccountName = 'st${appName}${environment}${substring(uniqueSuffix, 0, 4)}'
var serviceBusName = 'sb-${suffix}-${substring(uniqueSuffix, 0, 4)}'
var sqlServerName = 'sql-${suffix}-${substring(uniqueSuffix, 0, 4)}'
var sqlDatabaseName = 'sqldb-${suffix}'
var acrName = 'cr${appName}${environment}${substring(uniqueSuffix, 0, 4)}'
var containerAppEnvName = 'cae-${suffix}'

// Modules
module logAnalytics 'modules/log-analytics.bicep' = {
  name: 'logAnalyticsDeploy'
  params: {
    location: location
    workspaceName: logAnalyticsName
    tags: tags
  }
}

module appInsights 'modules/app-insights.bicep' = {
  name: 'appInsightsDeploy'
  params: {
    location: location
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    tags: tags
  }
}

module keyVault 'modules/key-vault.bicep' = {
  name: 'keyVaultDeploy'
  params: {
    location: location
    keyVaultName: keyVaultName
    tenantId: tenantId
    tags: tags
  }
}

module storage 'modules/storage.bicep' = {
  name: 'storageDeploy'
  params: {
    location: location
    storageAccountName: storageAccountName
    tags: tags
  }
}

module serviceBus 'modules/service-bus.bicep' = {
  name: 'serviceBusDeploy'
  params: {
    location: location
    serviceBusNamespaceName: serviceBusName
    tags: tags
  }
}

module sql 'modules/sql.bicep' = {
  name: 'sqlDeploy'
  params: {
    location: location
    sqlServerName: sqlServerName
    sqlDatabaseName: sqlDatabaseName
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
    tags: tags
  }
}

module containerRegistry 'modules/container-registry.bicep' = {
  name: 'acrDeploy'
  params: {
    location: location
    acrName: acrName
    tags: tags
  }
}

module containerAppEnv 'modules/container-apps.bicep' = {
  name: 'caeDeploy'
  params: {
    location: location
    environmentName: containerAppEnvName
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    logAnalyticsCustomerId: logAnalytics.outputs.workspaceCustomerId
    tags: tags
  }
}

// Outputs useful for deployment scripts
output containerRegistryLoginServer string = containerRegistry.outputs.acrLoginServer
output serviceBusNamespaceId string = serviceBus.outputs.serviceBusNamespaceId
output keyVaultUri string = keyVault.outputs.keyVaultUri
