using LooseFunds.Shared.Contracts.Investor.Commands;
using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Planner.Api.Workers;

public sealed class CreateInvestmentWorker : BackgroundService
{
    private const int DELAY_IN_H = 24;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreateInvestmentWorker> _logger;

    public CreateInvestmentWorker(IServiceProvider serviceProvider, ILogger<CreateInvestmentWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IOutboxStore store = scope.ServiceProvider.GetRequiredService<IOutboxStore>();

            await store.AddAsync(new CreateInvestmentCommand(), stoppingToken);
            TimeSpan delay = TimeSpan.FromHours(DELAY_IN_H);

            _logger.LogDebug("Finished execution [next_run_at={Next}]", DateTime.UtcNow.Add(delay));
            await Task.Delay(delay, stoppingToken);
        }
    }
}
