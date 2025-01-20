using LooseFunds.Shared.Toolbox.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LooseFunds.Shared.Toolbox.Jobs;

public static class JobsExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(q => { q.WaitForJobsToComplete = true; });
        return services;
    }

    public static IServiceCollection AddJob<TJob>(this IServiceCollection services) where TJob : class, IJob
    {
        services.AddTransient<TJob>();
        services.AddTransient<IJob>(serviceProvider =>
        {
            var baseJob = serviceProvider.GetRequiredService<TJob>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            var uowJob = new JobUnitOfWorkDecorator(baseJob, unitOfWork);
            return uowJob;
        });

        return services;
    }

    //TODO validate if jobs are added to the project
    public static async Task ScheduleJobAsync<TJob>(this WebApplication webApplication,
        Action<SimpleScheduleBuilder> scheduleBuilder) where TJob : IJob
    {
        var logger = webApplication.Services.GetRequiredService<ILogger<JobScheduler>>();
        
        var schedulerFactory = webApplication.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        var jobName = typeof(TJob).Name;
        var triggerName = $"{jobName}-trigger";
        var group = $"{jobName}-group";

        logger.LogTrace("Created job [job_name={JobName}, trigger_name={TriggerName}, group={Group}]", jobName,
            triggerName, group);

        var job = JobBuilder.Create<TJob>()
            .WithIdentity(jobName, group)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerName, group)
            .StartNow()
            .WithSimpleSchedule(scheduleBuilder)
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        logger.LogDebug("Scheduled job [job_name={JobName}]", jobName);
    }

    private sealed record JobScheduler;
}