using LooseFunds.Shared.Toolbox.Messaging;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LooseFunds.Planner.Application.Jobs;

public sealed class StartDailyInvestmentJob : IJob
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<StartDailyInvestmentJob> _logger;

    public StartDailyInvestmentJob(IMessagePublisher messagePublisher, ILogger<StartDailyInvestmentJob> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Send new create investment at {now}", DateTime.Now);
        await _messagePublisher.PublishAsync(new Rubbish(), context.CancellationToken);
    }
}