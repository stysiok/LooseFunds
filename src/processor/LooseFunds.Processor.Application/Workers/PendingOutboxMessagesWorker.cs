using LooseFunds.Processor.Application.Options;
using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LooseFunds.Processor.Application.Workers;

internal sealed class PendingOutboxMessagesWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PendingOutboxMessagesWorker> _logger;
    private readonly TimeSpan _delay;

    public PendingOutboxMessagesWorker(IServiceProvider serviceProvider, IOptions<PendingOutboxMessagesWorkerOptions> options, ILogger<PendingOutboxMessagesWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _delay = options.Value.Delay;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IOutboxProcessor processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

            await processor.SendPendingAsync(stoppingToken);

            _logger.LogDebug("Finished execution [next_run_at={Next}]", DateTime.UtcNow.Add(_delay));
            await Task.Delay(_delay, stoppingToken);
        }
    }
}
