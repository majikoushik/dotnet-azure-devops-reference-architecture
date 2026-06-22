namespace EnterpriseClaims.Contracts.Claims;

public sealed record ClaimSubmissionResponse(
    string ClaimNumber,
    string Status,
    DateTimeOffset SubmittedAt);
