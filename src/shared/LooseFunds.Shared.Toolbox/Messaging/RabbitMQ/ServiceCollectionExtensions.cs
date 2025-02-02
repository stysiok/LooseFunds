using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.Messaging.RabbitMQ;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumer<TMessage, TConsumer>(this IServiceCollection services)
        where TConsumer : RabbitMqConsumer<TMessage> where TMessage : class =>
        services.AddMassTransit(x => { x.AddConsumer<TConsumer>(); });

    internal static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMQOptions options = configuration.GetSection(RabbitMQConsts.RabbitMQSection).Get<RabbitMQOptions>() ??
                                  throw new Exception(); //validate
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(options.Host, "/", h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

        return services;
    }
}
