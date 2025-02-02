using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Converters;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

public static class ServiceCollectionExtensions
{
    //TODO validate if jobs are added, as outbox requires them
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddSingleton<IDomainObjectConverter<Models.Outbox, OutboxEntity>, OutboxConverter>();
        services.AddSingleton<IEntityObjectConverter<OutboxEntity, Models.Outbox>, OutboxConverter>();

        services.AddScoped<IOutboxProcessor, OutboxProcessor>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();

        return services;
    }
}
