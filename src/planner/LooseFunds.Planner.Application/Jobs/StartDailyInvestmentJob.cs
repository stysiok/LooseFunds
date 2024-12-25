using LooseFunds.Shared.Contracts.Investor.Commands;
using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LooseFunds.Planner.Application.Jobs;

public sealed class StartDailyInvestmentJob : IJob
{
    private readonly IOutboxService _outboxService;
    private readonly ILogger<StartDailyInvestmentJob> _logger;

    public StartDailyInvestmentJob(IOutboxService outboxService, ILogger<StartDailyInvestmentJob> logger)
    {
        _outboxService = outboxService;
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Publishing message [message_type={MessageType}]", nameof(CreateInvestmentCommand));

        _outboxService.Add(new CreateInvestmentCommand());

        _logger.LogInformation("Published message [message_type={MessageType}]", nameof(CreateInvestmentCommand));

        return Task.CompletedTask;
    }
}