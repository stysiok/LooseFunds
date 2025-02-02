using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Processor.Application.Workers;

public sealed class PendingOutboxMessagesWorker : BackgroundService
{
    private const int DELAY_IN_S = 10;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PendingOutboxMessagesWorker> _logger;

    public PendingOutboxMessagesWorker(IServiceProvider serviceProvider, ILogger<PendingOutboxMessagesWorker> logger)
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
            TimeSpan delay = TimeSpan.FromSeconds(DELAY_IN_S);

            _logger.LogDebug("Finished execution [next_run_at={Next}]", DateTime.UtcNow.Add(delay));
            await Task.Delay(delay, stoppingToken);
        }
    }
}
