using System.Reflection;
using System.Text;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Memphis.Client;
using Memphis.Client.Consumer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal sealed class MemphisMessageSubscriber : IMessageSubscriber
{
    private readonly ClientOptions _clientOptions;
    private readonly ILogger<MemphisMessageSubscriber> _logger;
    private readonly string _consumerName;
    private readonly string _consumerGroup;
    private const int Delay = 10_000;

    public MemphisMessageSubscriber(ClientOptions clientOptions, ILogger<MemphisMessageSubscriber> logger)
    {
        _clientOptions = clientOptions;
        _logger = logger;
        _consumerName = Guid.NewGuid().ToString("N")[..^8];
        _consumerGroup = Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown";
    }

    public async Task SubscribeAsync<TContent>(Recipient recipient, Action<TContent> onMessageReceived,
        CancellationToken cancellationToken) where TContent : IntegrationEvent
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

        consumer.MessageReceived += (_, args) =>
        {
            if (args.Exception != null)
            {
                _logger.LogError(
                    "Received exception from consumer on MessageReceived event [consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
                    _consumerName, _consumerGroup, stationName);
                throw args.Exception;
            }

            foreach (var msg in args.MessageList)
            {
                var data = msg.GetData();
                var json = Encoding.UTF8.GetString(data);

                _logger.LogDebug(
                    "Received message [message={Message}, consumer_name={ConsumerName}, consumer-group={ConsumerGroup}, station={Station}]",
                    json, _consumerName, _consumerGroup, stationName);
                var message = JsonConvert.DeserializeObject<TContent>(json) ?? throw new Exception();

                // print message headers
                // foreach (var headerKey in msg.GetHeaders().Keys)
                // {
                //     _logger.LogInformation(
                //         $"Header Key: {headerKey}, value: {msg.GetHeaders()[headerKey.ToString()]}");
                // }

                onMessageReceived(message);

                msg.Ack();
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