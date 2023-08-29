using Microsoft.Extensions.Logging;
using Quartz;

namespace LooseFunds.Planner.Application.Jobs;

public sealed class StartDailyInvestmentJob : IJob
{
    private readonly ILogger<StartDailyInvestmentJob> _logger;

    public StartDailyInvestmentJob(ILogger<StartDailyInvestmentJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Send new create investment at {now}", DateTime.Now);
        return Task.CompletedTask;
    }
}