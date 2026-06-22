using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Claims.Api.Data;

public sealed class ClaimsDbContextFactory : IDesignTimeDbContextFactory<ClaimsDbContext>
{
    public ClaimsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ClaimsDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EnterpriseClaims_DesignTime;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new ClaimsDbContext(options);
    }
}
