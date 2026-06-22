using Claims.Api.Domain;

namespace Claims.Api.Data;

public interface IClaimRepository
{
    Task AddAsync(ClaimRecord claim, CancellationToken cancellationToken);
}
