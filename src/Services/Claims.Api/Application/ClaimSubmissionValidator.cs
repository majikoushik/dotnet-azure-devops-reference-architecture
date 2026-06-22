using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.Contracts.Claims;

namespace Claims.Api.Application;

public sealed class ClaimSubmissionValidator
{
    public ValidationResult Validate(ClaimSubmissionRequest request)
    {
        var errors = new List<ApiError>();

        if (string.IsNullOrWhiteSpace(request.CustomerId))
        {
            errors.Add(ApiError.Validation("claim.customer_required", "CustomerId is required."));
        }

        if (string.IsNullOrWhiteSpace(request.PolicyNumber))
        {
            errors.Add(ApiError.Validation("claim.policy_required", "PolicyNumber is required."));
        }

        if (request.EstimatedAmount <= 0)
        {
            errors.Add(ApiError.Validation("claim.amount_invalid", "EstimatedAmount must be greater than zero."));
        }

        if (string.IsNullOrWhiteSpace(request.LossDescription))
        {
            errors.Add(ApiError.Validation("claim.description_required", "LossDescription is required."));
        }

        if (request.LossDescription?.Length > 500)
        {
            errors.Add(ApiError.Validation("claim.description_too_long", "LossDescription must be 500 characters or fewer."));
        }

        return errors.Count == 0 ? ValidationResult.Success() : new ValidationResult(errors);
    }
}
