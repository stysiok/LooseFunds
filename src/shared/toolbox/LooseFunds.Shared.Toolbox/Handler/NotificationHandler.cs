using LooseFunds.Shared.Toolbox.UnitOfWork;
using MediatR;

namespace LooseFunds.Shared.Toolbox.Handler;

internal sealed class UnitOfWorkNotificationHandler<T> : INotificationHandler<T> where T : INotification
{
    private readonly INotificationHandler<T> _handler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkNotificationHandler(INotificationHandler<T> handler, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = handler;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await _handler.Handle(notification, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}