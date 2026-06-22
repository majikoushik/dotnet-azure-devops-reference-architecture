using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EnterpriseClaims.BuildingBlocks.Security;

public static class SecurityExtensions
{
    public static IServiceCollection AddEnterpriseSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"];
        if (string.IsNullOrWhiteSpace(key) || key == "<Set_In_User_Secrets>")
        {
            throw new InvalidOperationException("JWT Key is missing from configuration. Set it via user secrets or environment variables.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "EnterpriseClaimsLocal",
                    ValidAudience = jwtSettings["Audience"] ?? "EnterpriseClaimsApi",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
            options.AddPolicy("ClaimProcessor", policy => policy.RequireRole("ClaimProcessor"));
            options.AddPolicy("Supervisor", policy => policy.RequireRole("Supervisor"));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}
