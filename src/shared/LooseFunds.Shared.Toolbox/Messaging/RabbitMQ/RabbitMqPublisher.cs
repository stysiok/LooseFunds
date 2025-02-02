using LooseFunds.Shared.Toolbox.Messaging.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.RabbitMQ;

internal sealed class RabbitMqPublisher : IMessagePublisher
{
    private readonly IBus _bus;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IBus bus, ILogger<RabbitMqPublisher> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task PublishAsync(PublishMessage publishMessage, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Sending message to station [message_id={MessageId}, message_type={MessageType}, station={Station}]",
            publishMessage.Id, publishMessage.Type, publishMessage.Recipient);

        await _bus.Publish(publishMessage, cancellationToken);

        _logger.LogDebug(
            "Sent message to station [message_id={MessageId}, message_type={MessageType}, station={Station}]",
            publishMessage.Id, publishMessage.Type, publishMessage.Recipient);
    }
}
