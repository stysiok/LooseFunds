using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging;

public interface IMessageSubscriber
{
    Task SubscribeAsync<TContent>(Recipient recipient, Func<TContent, Task> onMessageReceived,
        CancellationToken cancellationToken) where TContent : IMessageContent;
}