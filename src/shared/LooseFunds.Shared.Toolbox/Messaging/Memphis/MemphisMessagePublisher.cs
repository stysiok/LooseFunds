using LooseFunds.Shared.Toolbox.Correlation;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal sealed class MemphisMessagePublisher : IMessagePublisher
{
    private readonly IMemphisProducerProvider _memphisProducerProvider;
    private readonly ICorrelationAccessor _correlationAccessor;
    private readonly ILogger<MemphisMessagePublisher> _logger;

    public MemphisMessagePublisher(IMemphisProducerProvider memphisProducerProvider,
        ICorrelationAccessor correlationAccessor, ILogger<MemphisMessagePublisher> logger)
    {
        _memphisProducerProvider = memphisProducerProvider;
        _correlationAccessor = correlationAccessor;
        _logger = logger;
    }

    public async Task PublishAsync<TMessage>(PublishMessage<TMessage> messageBase,
        CancellationToken cancellationToken) where TMessage : IMessageContent
    {
        _logger.LogDebug("Sending message to station [message_type={MessageType}, station={Station}]",
            typeof(TMessage).Name, messageBase.Recipient);

        var producer = await _memphisProducerProvider.GetProducerAsync(messageBase.Recipient, cancellationToken);
        var bytes = messageBase.ToBytes();

        var headers = MessageMetadata.Generate(_correlationAccessor.CorrelationId, typeof(TMessage).Name).ToHeaders();

        await producer.ProduceAsync(bytes, headers);

        _logger.LogDebug("Sent message to station [message_type={MessageType}, station={Station}]",
            typeof(TMessage).Name, messageBase.Recipient);
    }
}