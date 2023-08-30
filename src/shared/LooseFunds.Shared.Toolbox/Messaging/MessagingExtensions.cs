using System.Collections.Specialized;
using System.Text;
using Memphis.Client;
using Memphis.Client.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MemphisMessagePublisher>();
        return services;
    }
}

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : IMessage;
}

public interface IMessageSubscriber
{
    Task SubscribeAsync<TMessage>(Action<TMessage> action, CancellationToken cancellationToken)
        where TMessage : IMessage;
}

public interface IMessage
{
}

public class Rubbish : IMessage
{
}

internal sealed class MemphisMessagePublisher : IMessagePublisher
{
    private readonly ILogger<MemphisMessagePublisher> _logger;
    private readonly ClientOptions _options;

    public MemphisMessagePublisher(ILogger<MemphisMessagePublisher> logger)
    {
        _logger = logger;
        var options = MemphisClientFactory.GetDefaultOptions();
        options.Host = "localhost";
        options.Username = "root";
        options.Password = "memphis";
        options.AccountId = 1;
        _options = options;
    }


    //TODO make it better
    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : IMessage
    {
        var client = await MemphisClientFactory.CreateClient(_options, cancellationToken);
        var producer = await client.CreateProducer(new MemphisProducerOptions
        {
            StationName = "loosefunds",
            ProducerName = "greece",
            GenerateUniqueSuffix = true
        });
        var rubbish = "rubbish";
        await producer.ProduceAsync(Encoding.UTF8.GetBytes(rubbish), new NameValueCollection());
    }
}