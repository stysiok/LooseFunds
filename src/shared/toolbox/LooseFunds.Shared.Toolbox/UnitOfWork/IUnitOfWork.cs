using System.Collections.Concurrent;
using LooseFunds.Shared.Toolbox.Domain;
using LooseFunds.Shared.Toolbox.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    void Add(Entity entity);
    Entity? Get(Guid id);
}

internal interface IProcessableUnitOfWork
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

internal sealed class UnitOfWork : IUnitOfWork, IProcessableUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly ConcurrentDictionary<Guid, Entity> _entries = new();

    public UnitOfWork(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Add(Entity entity)
    {
        _entries.TryAdd(entity.Id, entity);
    }

    public Entity? Get(Guid id)
    {
        return _entries.TryGetValue(id, out var entry) ? entry : null;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        foreach (var entity in _entries.Values)
        {
            var domainEvent = entity.TryGetNextDomainEvent();
            while (domainEvent is not null)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
                domainEvent = entity.TryGetNextDomainEvent();
            }
        }
    }
}

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>()
    .Decorate(typeof(INotificationHandler<>), typeof(UnitOfWorkNotificationHandler<>));
}