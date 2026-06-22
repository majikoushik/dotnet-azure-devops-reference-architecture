namespace EnterpriseClaims.Contracts.Customers;

public sealed record CustomerSummaryResponse(
    string CustomerId,
    string FullName,
    string EmailAddress,
    string Status);
