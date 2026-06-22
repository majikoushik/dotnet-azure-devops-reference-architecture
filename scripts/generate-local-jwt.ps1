<#
.SYNOPSIS
Generates a local JWT for testing the Enterprise Claims API.

.DESCRIPTION
Reads the local .env file for JWT configuration and generates a token valid for 24 hours.
The token includes the 'Customer' and 'ClaimProcessor' roles.
#>

$envFile = Join-Path $PSScriptRoot "..\.env"

if (-not (Test-Path $envFile)) {
    Write-Error "Cannot find .env file at $envFile. Please copy .env.example to .env and configure it."
    exit 1
}

# Parse .env file
$envVars = @{}
Get-Content $envFile | Where-Object { $_ -match '^([^#]+?)=(.*)$' } | ForEach-Object {
    $envVars[$matches[1].Trim()] = $matches[2].Trim()
}

$jwtKey = $envVars['JWT_KEY']
$jwtIssuer = $envVars['JWT_ISSUER']
$jwtAudience = $envVars['JWT_AUDIENCE']

if (-not $jwtKey -or $jwtKey -match '<set-local-jwt-key-here>') {
    Write-Error "Please set a valid JWT_KEY in the .env file."
    exit 1
}

# Provide fallback for Issuer/Audience if empty
if (-not $jwtIssuer) { $jwtIssuer = "EnterpriseClaimsLocal" }
if (-not $jwtAudience) { $jwtAudience = "EnterpriseClaimsApi" }

# Build a small C# inline script to generate the token
$csharpCode = @"
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper
{
    public static string Generate(string key, string issuer, string audience)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "local-tester"),
            new Claim(JwtRegisteredClaimNames.Email, "tester@example.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("customerId", "CUST-LOCAL-001"),
            new Claim(ClaimTypes.Role, "Customer"),
            new Claim(ClaimTypes.Role, "ClaimProcessor")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
"@

Add-Type -TypeDefinition $csharpCode -ReferencedAssemblies "System.IdentityModel.Tokens.Jwt", "Microsoft.IdentityModel.Tokens"

$token = [JwtHelper]::Generate($jwtKey, $jwtIssuer, $jwtAudience)

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Generated JWT for Local Testing:" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host $token
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Roles: Customer, ClaimProcessor"
Write-Host "CustomerId: CUST-LOCAL-001"
Write-Host "Expires: 24 hours from now"
