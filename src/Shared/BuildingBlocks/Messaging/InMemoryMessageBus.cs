using System.Collections.Concurrent;

namespace EnterpriseClaims.BuildingBlocks.Messaging;

public sealed class InMemoryMessageBus : IMessagePublisher
{
    private readonly ConcurrentQueue<object> _messages = new();

    public IReadOnlyCollection<object> PublishedMessages => _messages.ToArray();

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _messages.Enqueue(message!);
        return Task.CompletedTask;
    }
}
