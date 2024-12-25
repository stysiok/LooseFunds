using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging;

public interface IMessageSubscriber
{
    Task SubscribeAsync<TContent>(Recipient recipient, Action<TContent> onMessageReceived,
        CancellationToken cancellationToken) where TContent : IntegrationEvent;
}