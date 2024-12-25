using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync(PublishMessage publishMessage, CancellationToken cancellationToken);
}