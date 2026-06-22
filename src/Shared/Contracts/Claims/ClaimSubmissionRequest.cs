namespace EnterpriseClaims.Contracts.Claims;

public sealed record ClaimSubmissionRequest(
    string CustomerId,
    string PolicyNumber,
    decimal EstimatedAmount,
    string LossDescription);
