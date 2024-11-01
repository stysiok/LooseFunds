using LooseFunds.Shared.Toolbox.Messaging.Memphis;
using Memphis.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.Messaging;

public static class MessagingExtensions
{
    //TODO validate settings
    //TODO make obsolete and find alternative for rpi (rabbitmq in docker)

    public static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var memphisOptions = configuration.GetSection(MessagingConsts.MemphisSection).Get<MemphisOptions>();

        services.AddSingleton<ClientOptions>(_ =>
        {
            var options = MemphisClientFactory.GetDefaultOptions();
            options.Host = memphisOptions.Host;
            options.Username = memphisOptions.Username;
            options.Password = memphisOptions.Password;
            options.AccountId = memphisOptions.AccountId;
            return options;
        });

        services.AddSingleton<IMessagePublisher, MemphisMessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MemphisMessageSubscriber>();
        services.AddSingleton<IMemphisProducerProvider, MemphisProducerProvider>();
    }
}