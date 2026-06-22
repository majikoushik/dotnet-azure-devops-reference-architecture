using EnterpriseClaims.BuildingBlocks;
using EnterpriseClaims.Contracts.Claims;

namespace Claims.Api.Application;

public sealed record ClaimSubmissionResult(
    ValidationResult Validation,
    ClaimSubmissionResponse? Response)
{
    public static ClaimSubmissionResult Rejected(ValidationResult validation) => new(validation, null);

    public static ClaimSubmissionResult Accepted(ClaimSubmissionResponse response) => new(ValidationResult.Success(), response);
}
