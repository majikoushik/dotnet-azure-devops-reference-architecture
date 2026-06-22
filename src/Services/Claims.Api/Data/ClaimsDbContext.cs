using Claims.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Data;

public sealed class ClaimsDbContext(DbContextOptions<ClaimsDbContext> options) : DbContext(options)
{
    public DbSet<ClaimRecord> Claims => Set<ClaimRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var claim = modelBuilder.Entity<ClaimRecord>();

        claim.ToTable("Claims");
        claim.HasKey(x => x.Id);
        claim.Property(x => x.ClaimNumber).HasMaxLength(32).IsRequired();
        claim.HasIndex(x => x.ClaimNumber).IsUnique();
        claim.Property(x => x.CustomerId).HasMaxLength(64).IsRequired();
        claim.Property(x => x.PolicyNumber).HasMaxLength(64).IsRequired();
        claim.Property(x => x.EstimatedAmount).HasPrecision(18, 2);
        claim.Property(x => x.LossDescription).HasMaxLength(500).IsRequired();
        claim.Property(x => x.Status).HasMaxLength(32).IsRequired();
        claim.Property(x => x.SubmittedAt).IsRequired();
    }
}
