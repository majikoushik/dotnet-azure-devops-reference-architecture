using Claims.Api.Domain;

namespace Claims.Api.Data;

public sealed class EfClaimRepository(ClaimsDbContext dbContext) : IClaimRepository
{
    public async Task AddAsync(ClaimRecord claim, CancellationToken cancellationToken)
    {
        await dbContext.Claims.AddAsync(claim, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
