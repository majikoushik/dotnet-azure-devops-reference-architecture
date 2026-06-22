using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Data;

public static class ClaimsPersistenceExtensions
{
    public static IServiceCollection AddClaimsPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ClaimsDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<ClaimsDbContext>(options => options.UseInMemoryDatabase("claims-local"));
        }
        else
        {
            services.AddDbContext<ClaimsDbContext>(options => options.UseSqlServer(connectionString));
        }

        services.AddScoped<IClaimRepository, EfClaimRepository>();

        return services;
    }
}
