using LooseFunds.Shared.Toolbox.UnitOfWork;
using MediatR;

namespace LooseFunds.Shared.Toolbox.Handler;

internal sealed class UnitOfWorkNotificationHandler<T> : INotificationHandler<T> where T : INotification
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationHandler<T> _handler;

    public UnitOfWorkNotificationHandler(INotificationHandler<T> handler, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = handler;
    }
    
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await _handler.Handle(notification, cancellationToken);
        
        var processableUnitOfWork = (IProcessableUnitOfWork)_unitOfWork;
        await processableUnitOfWork.ProcessAsync(cancellationToken);
    }
}