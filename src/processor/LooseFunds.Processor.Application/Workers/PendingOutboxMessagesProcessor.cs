using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Processor.Application.Workers;

public sealed class OutboxWorker : BackgroundService
{
    private const int DELAY = 10;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxWorker> _logger;

    public OutboxWorker(IServiceProvider serviceProvider, ILogger<OutboxWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IOutboxProcessor processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

            await processor.SendPendingAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
