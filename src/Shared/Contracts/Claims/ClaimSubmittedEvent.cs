namespace EnterpriseClaims.Contracts.Claims;

public sealed record ClaimSubmittedEvent(
    string ClaimNumber,
    string CustomerId,
    string PolicyNumber,
    decimal EstimatedAmount,
    DateTimeOffset SubmittedAt);
