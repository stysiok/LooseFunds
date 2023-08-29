using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LooseFunds.Shared.Toolbox.Jobs;

public static class JobsExtensions
{
    public static void AddJobs(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(q => { q.WaitForJobsToComplete = true; });
    }

    public static async Task ScheduleJobAsync<TJob>(this WebApplication webApplication) where TJob : IJob
    {
        var schedulerFactory = webApplication.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        var jobName = typeof(TJob).Name;
        var triggerName = $"{jobName}-trigger";
        var group = $"{jobName}-group";

        //TODO Log stuff

        var job = JobBuilder.Create<TJob>()
            .WithIdentity(jobName, group)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerName, group)
            .StartNow()
            .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(4))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}