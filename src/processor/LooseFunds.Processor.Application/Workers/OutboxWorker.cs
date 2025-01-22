using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Processor.Application.Workers;

public sealed class OutboxWorker : BackgroundService
{
    private readonly ILogger<OutboxWorker> _logger;

    public OutboxWorker(ILogger<OutboxWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            _logger.LogInformation("working... {now}", DateTime.Now.ToShortTimeString());

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
