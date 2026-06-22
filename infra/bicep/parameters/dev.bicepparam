using '../main.bicep'

param location = 'eastus'
param environment = 'dev'
param appName = 'entclaims'

// Safe placeholders for dev environment
param sqlAdminLogin = 'ClaimsDbAdmin'
