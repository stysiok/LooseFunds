using LooseFunds.Shared.Contracts.Investor;
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
        _logger.LogInformation("Publishing message [message_type={MessageType}]", nameof(CreateInvestmentCommand));

        await _messagePublisher.PublishAsync(CreateInvestmentCommand.BuildMessage(), context.CancellationToken);

        _logger.LogInformation("Published message [message_type={MessageType}]", nameof(CreateInvestmentCommand));
    }
}