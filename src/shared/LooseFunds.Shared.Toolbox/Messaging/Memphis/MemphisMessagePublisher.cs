using System.Collections.Specialized;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal sealed class MemphisMessagePublisher : IMessagePublisher
{
    private readonly IMemphisProducerProvider _memphisProducerProvider;
    private readonly ILogger<MemphisMessagePublisher> _logger;

    public MemphisMessagePublisher(IMemphisProducerProvider memphisProducerProvider,
        ILogger<MemphisMessagePublisher> logger)
    {
        _memphisProducerProvider = memphisProducerProvider;
        _logger = logger;
    }

    public async Task PublishAsync(PublishMessage messageBase,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Sending message to station [message_id={MessageId}, message_type={MessageType}, station={Station}]",
            messageBase.Id, messageBase.Type, messageBase.Recipient);

        var producer = await _memphisProducerProvider.GetProducerAsync(messageBase.Recipient, cancellationToken);
        var bytes = messageBase.ToBytes();
        await producer.ProduceAsync(bytes, new NameValueCollection());

        _logger.LogDebug(
            "Sent message to station [message_id={MessageId}, message_type={MessageType}, station={Station}]",
            messageBase.Id, messageBase.Type, messageBase.Recipient);
    }
}