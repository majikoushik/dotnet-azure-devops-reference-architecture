using Claims.Api.Application;
using EnterpriseClaims.Contracts.Claims;

namespace EnterpriseClaims.UnitTests.Claims;

public sealed class ClaimSubmissionValidatorTests
{
    private readonly ClaimSubmissionValidator _validator = new();

    [Fact]
    public void Validate_ReturnsSuccess_WhenRequestIsComplete()
    {
        var request = new ClaimSubmissionRequest(
            "CUST-1001",
            "POL-2026-0001",
            1250.50m,
            "Water damage to kitchen flooring.");

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_ReturnsErrors_WhenRequiredFieldsAreMissing()
    {
        var request = new ClaimSubmissionRequest("", "", 0, "");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Code == "claim.customer_required");
        Assert.Contains(result.Errors, error => error.Code == "claim.policy_required");
        Assert.Contains(result.Errors, error => error.Code == "claim.amount_invalid");
        Assert.Contains(result.Errors, error => error.Code == "claim.description_required");
    }

    [Fact]
    public void Validate_ReturnsError_WhenDescriptionIsTooLong()
    {
        var request = new ClaimSubmissionRequest(
            "CUST-1001",
            "POL-2026-0001",
            500m,
            new string('x', 501));

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Code == "claim.description_too_long");
    }
}
