using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Jobs;
using LooseFunds.Shared.Toolbox.Messaging.Memphis;
using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Converters;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using Memphis.Client;
using Microsoft.AspNetCore.Builder;
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

    //TODO validate if jobs are added, as outbox requires them 
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddSingleton<IDomainObjectConverter<Outbox.Models.Outbox, OutboxEntity>, OutboxConverter>();
        services.AddSingleton<IEntityObjectConverter<OutboxEntity, Outbox.Models.Outbox>, OutboxConverter>();

        services.AddJob<OutboxProcessor>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IOutboxStore, OutboxRepository>();

        return services;
    }

    public static Task ScheduleOutboxJob(this WebApplication webApplication)
        => webApplication.ScheduleJobAsync<OutboxProcessor>(b => b.WithIntervalInSeconds(30).RepeatForever());
}