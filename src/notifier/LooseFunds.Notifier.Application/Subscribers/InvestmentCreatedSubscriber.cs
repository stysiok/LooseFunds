using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Notifier.Application.Subscribers;

public sealed class InvestmentCreatedEventSubscriber : BackgroundService
{
    private readonly ILogger<InvestmentCreatedEventSubscriber> _logger;

    public InvestmentCreatedEventSubscriber(ILogger<InvestmentCreatedEventSubscriber> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("I'm running");

        return Task.CompletedTask;
    }
}
