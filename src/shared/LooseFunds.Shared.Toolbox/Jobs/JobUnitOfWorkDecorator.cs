using LooseFunds.Shared.Toolbox.UnitOfWork;
using Quartz;

namespace LooseFunds.Shared.Toolbox.Jobs;

internal sealed class JobUnitOfWorkDecorator : IJob
{
    private readonly IJob _job;
    private readonly IUnitOfWork _unitOfWork;

    public JobUnitOfWorkDecorator(IJob job, IUnitOfWork unitOfWork)
    {
        _job = job;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _job.Execute(context);
        await _unitOfWork.CommitAsync();
    }
}