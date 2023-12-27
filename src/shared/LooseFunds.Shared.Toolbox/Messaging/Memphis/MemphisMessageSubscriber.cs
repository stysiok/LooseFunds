using System.Reflection;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Memphis.Client;
using Memphis.Client.Consumer;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal sealed class MemphisMessageSubscriber : IMessageSubscriber
{
    private readonly ClientOptions _clientOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MemphisMessageSubscriber> _logger;
    private readonly string _consumerName;
    private readonly string _consumerGroup;
    private const int Delay = 10_000;

    public MemphisMessageSubscriber(ClientOptions clientOptions, IServiceProvider serviceProvider,
        ILogger<MemphisMessageSubscriber> logger)
    {
        _clientOptions = clientOptions;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _consumerName = Guid.NewGuid().ToString("N")[..^8];
        _consumerGroup = Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown";
    }

    public async Task SubscribeAsync<TContent>(Recipient recipient, Func<TContent, Task> onMessageReceived,
        CancellationToken cancellationToken) where TContent : IMessageContent
    {
        var stationName = recipient.ToString();

        var client = await MemphisClientFactory.CreateClient(_clientOptions, cancellationToken);
        var consumer = await client.CreateConsumer(new MemphisConsumerOptions
        {
            StationName = stationName,
            ConsumerGroup = _consumerGroup,
            ConsumerName = _consumerName
        });

        _logger.LogDebug(
            "Created consumer [consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
            _consumerName, _consumerGroup, stationName);

        consumer.MessageReceived += async (_, args) =>
        {
            if (args.Exception != null)
            {
                _logger.LogError(
                    "Received exception from consumer on MessageReceived event [consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
                    _consumerName, _consumerGroup, stationName);
                throw args.Exception;
            }

            foreach (var message in args.MessageList)
            {
                var content = message.ToContent<TContent>();
                var metadata = message.GetHeaders().ToMessageMetadata();

                _logger.LogDebug(
                    "Received message [message_id={MessageId}, message_type={MessageType}, consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
                    metadata.MessageId, metadata.MessageType, _consumerName, _consumerGroup, stationName);

                message.Ack();
                _logger.LogInformation(
                    "Destroyed consumer [consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
                    _consumerName, _consumerGroup, stationName);
            }
        };

        await consumer.ConsumeAsync(cancellationToken);

        await Task.Delay(Delay, cancellationToken);
        await consumer.DestroyAsync();
    }
}