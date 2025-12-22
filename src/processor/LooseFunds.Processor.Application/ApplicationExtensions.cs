using LooseFunds.Processor.Application.Options;
using LooseFunds.Processor.Application.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Processor.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<PendingOutboxMessagesWorkerOptions>(configuration.GetSection("Workers:PendingOutboxMessagesWorker"))
            .AddHostedService<PendingOutboxMessagesWorker>();
        return services;
    }
}
