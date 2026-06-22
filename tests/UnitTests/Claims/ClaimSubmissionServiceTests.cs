using Claims.Api.Application;
using Claims.Api.Data;
using Claims.Api.Domain;
using EnterpriseClaims.BuildingBlocks.Messaging;
using EnterpriseClaims.Contracts.Claims;

namespace EnterpriseClaims.UnitTests.Claims;

public sealed class ClaimSubmissionServiceTests
{
    [Fact]
    public async Task SubmitAsync_PersistsClaimAndPublishesEvent_WhenRequestIsValid()
    {
        var repository = new CapturingClaimRepository();
        var publisher = new CapturingMessagePublisher();
        var service = new ClaimSubmissionService(
            new ClaimSubmissionValidator(),
            new ClaimNumberGenerator(),
            repository,
            publisher);

        var request = new ClaimSubmissionRequest(
            " CUST-1001 ",
            " POL-2026-0001 ",
            1250.50m,
            " Water damage to kitchen flooring. ");

        var result = await service.SubmitAsync(request, CancellationToken.None);

        Assert.True(result.Validation.IsValid);
        Assert.NotNull(result.Response);
        Assert.NotNull(repository.Claim);
        Assert.Equal("CUST-1001", repository.Claim.CustomerId);
        Assert.Equal("POL-2026-0001", repository.Claim.PolicyNumber);
        Assert.Equal("Water damage to kitchen flooring.", repository.Claim.LossDescription);
        Assert.Equal("Submitted", repository.Claim.Status);

        var publishedEvent = Assert.IsType<ClaimSubmittedEvent>(publisher.Message);
        Assert.Equal(repository.Claim.ClaimNumber, publishedEvent.ClaimNumber);
        Assert.Equal(repository.Claim.CustomerId, publishedEvent.CustomerId);
        Assert.Equal(repository.Claim.PolicyNumber, publishedEvent.PolicyNumber);
    }

    [Fact]
    public async Task SubmitAsync_DoesNotPersistOrPublish_WhenRequestIsInvalid()
    {
        var repository = new CapturingClaimRepository();
        var publisher = new CapturingMessagePublisher();
        var service = new ClaimSubmissionService(
            new ClaimSubmissionValidator(),
            new ClaimNumberGenerator(),
            repository,
            publisher);

        var result = await service.SubmitAsync(new ClaimSubmissionRequest("", "", 0, ""), CancellationToken.None);

        Assert.False(result.Validation.IsValid);
        Assert.Null(result.Response);
        Assert.Null(repository.Claim);
        Assert.Null(publisher.Message);
    }

    private sealed class CapturingClaimRepository : IClaimRepository
    {
        public ClaimRecord? Claim { get; private set; }

        public Task AddAsync(ClaimRecord claim, CancellationToken cancellationToken)
        {
            Claim = claim;
            return Task.CompletedTask;
        }
    }

    private sealed class CapturingMessagePublisher : IMessagePublisher
    {
        public object? Message { get; private set; }

        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        {
            Message = message;
            return Task.CompletedTask;
        }
    }
}
