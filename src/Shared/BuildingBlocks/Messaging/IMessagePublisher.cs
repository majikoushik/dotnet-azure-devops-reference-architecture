namespace EnterpriseClaims.BuildingBlocks.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken);
}
