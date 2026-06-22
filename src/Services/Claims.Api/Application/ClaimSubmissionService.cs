using Claims.Api.Data;
using Claims.Api.Domain;
using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.Contracts.Claims;

namespace Claims.Api.Application;

public sealed class ClaimSubmissionService(
    ClaimSubmissionValidator validator,
    ClaimNumberGenerator claimNumberGenerator,
    IClaimRepository claimRepository,
    IMessagePublisher messagePublisher)
{
    public async Task<ClaimSubmissionResult> SubmitAsync(ClaimSubmissionRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsValid)
        {
            return ClaimSubmissionResult.Rejected(validation);
        }

        var submittedAt = DateTimeOffset.UtcNow;
        var claim = new ClaimRecord
        {
            Id = Guid.NewGuid(),
            ClaimNumber = claimNumberGenerator.Create(),
            CustomerId = request.CustomerId.Trim(),
            PolicyNumber = request.PolicyNumber.Trim(),
            EstimatedAmount = request.EstimatedAmount,
            LossDescription = request.LossDescription.Trim(),
            Status = "Submitted",
            SubmittedAt = submittedAt
        };

        await claimRepository.AddAsync(claim, cancellationToken);

        await messagePublisher.PublishAsync(
            new ClaimSubmittedEvent(
                claim.ClaimNumber,
                claim.CustomerId,
                claim.PolicyNumber,
                claim.EstimatedAmount,
                claim.SubmittedAt),
            cancellationToken);

        return ClaimSubmissionResult.Accepted(new ClaimSubmissionResponse(claim.ClaimNumber, claim.Status, claim.SubmittedAt));
    }
}
