using LooseFunds.Planner.Application.Jobs;
using LooseFunds.Shared.Toolbox.Correlation;
using LooseFunds.Shared.Toolbox.Jobs;
using LooseFunds.Shared.Toolbox.Logging;
using LooseFunds.Shared.Toolbox.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorrelationLogEnricher()
    .AddJobs()
    .AddMessaging(builder.Configuration);
    

builder.Host.UseLogging(builder.Configuration);

var app = builder.Build();

await app.ScheduleJobAsync<StartDailyInvestmentJob>();

app.Run();