namespace Claims.Api.Domain;

public sealed class ClaimRecord
{
    public Guid Id { get; set; }

    public required string ClaimNumber { get; set; }

    public required string CustomerId { get; set; }

    public required string PolicyNumber { get; set; }

    public decimal EstimatedAmount { get; set; }

    public required string LossDescription { get; set; }

    public required string Status { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }
}
