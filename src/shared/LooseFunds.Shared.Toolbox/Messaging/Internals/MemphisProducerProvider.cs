using System.Collections.Concurrent;
using System.Reflection;
using Memphis.Client;
using Memphis.Client.Producer;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging;

internal sealed class MemphisProducerProvider : IMemphisProducerProvider
{
    private readonly ConcurrentDictionary<string, MemphisProducer> _memphisProducers = new();
    private readonly ClientOptions _clientOptions;
    private readonly ILogger<MemphisMessagePublisher> _logger;
    private readonly string _producerName;

    public MemphisProducerProvider(ClientOptions clientOptions, ILogger<MemphisMessagePublisher> logger)
    {
        _clientOptions = clientOptions;
        _logger = logger;
        _producerName = $"{Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown"}-{Guid.NewGuid().ToString()[..8]}";
    }

    public async Task<MemphisProducer> GetProducerAsync(Recipient recipient, CancellationToken cancellationToken)
    {
        var stationName = recipient.ToString();
        if (_memphisProducers.TryGetValue(stationName, out var cachedProducer))
        {
            _logger.LogTrace("Found producer [station={Station}]", stationName);
            return cachedProducer;
        }

        _logger.LogTrace("Producer not found [station={Station}]", stationName);

        var client = await MemphisClientFactory.CreateClient(_clientOptions, cancellationToken);
        var producer = await client.CreateProducer(new MemphisProducerOptions
        {
            StationName = stationName,
            ProducerName = _producerName
        });
        _memphisProducers.AddOrUpdate(stationName, producer, (_, newProducer) => newProducer);

        _logger.LogTrace("Producer created and added [station={Station}]", stationName);

        return producer;
    }
}