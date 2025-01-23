using LooseFunds.Planner.Application.Jobs;
using LooseFunds.Shared.Toolbox.Correlation;
using LooseFunds.Shared.Toolbox.Jobs;
using LooseFunds.Shared.Toolbox.Logging;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Storage.Marten;
using LooseFunds.Shared.Toolbox.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorrelationLogEnricher()
    .AddJobs()
    .AddJob<StartDailyInvestmentJob>()
    .AddOutbox()
    .AddUnitOfWork()
    .AddMartenStorage(builder.Configuration, builder.Environment)
    .AddMessaging(builder.Configuration);


builder.Host.UseLogging(builder.Configuration);

var app = builder.Build();

await app.ScheduleJobAsync<StartDailyInvestmentJob>(b => b.RepeatForever().WithIntervalInHours(24));

app.Run();
