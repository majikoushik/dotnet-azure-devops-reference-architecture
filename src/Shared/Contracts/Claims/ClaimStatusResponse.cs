namespace EnterpriseClaims.Contracts.Claims;

public sealed record ClaimStatusResponse(
    string ClaimNumber,
    string CustomerId,
    string PolicyNumber,
    decimal EstimatedAmount,
    string Status,
    string LossDescription,
    DateTimeOffset SubmittedAt);
