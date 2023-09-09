namespace LooseFunds.Shared.Toolbox.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(PublishMessage<TMessage> publishMessage, CancellationToken cancellationToken)
        where TMessage : IMessageContent;
}