using System.Collections.Concurrent;
using LooseFunds.Shared.Toolbox.Domain;
using LooseFunds.Shared.Toolbox.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    void Add(DomainObject domainObject);
}

internal interface IProcessableUnitOfWork
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

internal sealed class UnitOfWork : IUnitOfWork, IProcessableUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly IOutbox _outbox;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly IIntegrationEventFactory? _integrationEventFactory;
    private readonly ConcurrentDictionary<Guid, DomainObject> _entries = new();

    public UnitOfWork(IMediator mediator, IOutbox outbox, IServiceProvider serviceProvider, ILogger<UnitOfWork> logger, IIntegrationEventFactory? integrationEventFactory = null)
    {
        _mediator = mediator;
        _outbox = outbox;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _integrationEventFactory = integrationEventFactory;
    }

    public void Add(DomainObject domainObject)
    {
        _entries.TryAdd(domainObject.Id, domainObject);
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        await ProcessDomainEvents(cancellationToken);
        await Commit(cancellationToken);
    }

    private async Task ProcessDomainEvents(CancellationToken cancellationToken)
    {
        foreach (var entity in _entries.Values)
        {
            var domainEvent = entity.TryGetNextDomainEvent();
            while (domainEvent is not null)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
                var integrationEvent = _integrationEventFactory?.BuildIntegrationEvent(domainEvent);
                if (integrationEvent is not null)
                {
                    _outbox.Add(integrationEvent);
                }
                domainEvent = entity.TryGetNextDomainEvent();
            }
        }
    }

    private async Task Commit(CancellationToken cancellationToken)
    {
        foreach (var (key, value) in _entries)
        {
            using var scope = _serviceProvider.CreateScope();
            var persistenceProvider = _serviceProvider.GetRequiredService<IPersistenceProvider>();
            _logger.LogInformation("Saving... id={Id}", key);
            await persistenceProvider.StoreAsync(value, cancellationToken);
            _logger.LogInformation("Saved id={id}", key);
        }
    }
}

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>()
    .Decorate(typeof(INotificationHandler<>), typeof(UnitOfWorkNotificationHandler<>));
}

public interface IIntegrationEventFactory
{
    IntegrationEvent BuildIntegrationEvent(IDomainEvent domainEvent);
}

public abstract record IntegrationEvent
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public DateTime Created { get; private init; } = DateTime.UtcNow;
}

public interface IOutbox
{
    void Add(IntegrationEvent integrationEvent);
}

public interface IPersistenceProvider
{
    Task StoreAsync(DomainObject domainObject, CancellationToken cancellationToken = default);
}

public abstract class PersistenceProviderBase<TDomain, TEntity> : IPersistenceProvider 
    where TDomain : DomainObject
    where TEntity : StorageEntity
{
    protected readonly IDomainConverter<TDomain, TEntity> DomainConverter;
    private readonly IStorage _storage;
    protected readonly ILogger Logger;

    public PersistenceProviderBase(IDomainConverter<TDomain, TEntity> domainConverter, IStorage storage, ILogger logger)
    {
        DomainConverter = domainConverter;
        _storage = storage;
        Logger = logger;
    }


    public async Task StoreAsync(DomainObject domainObject, CancellationToken cancellationToken)
    {
        var entity = DomainConverter.ToEntity((TDomain)domainObject);
        Logger.LogDebug("Converted from {DomainObject} to {StorageEntity}", nameof(DomainObject),
            nameof(StorageEntity));
        try
        {
            Logger.LogDebug("Starting save to the {Storage}", nameof(IStorage));
            await _storage.WriteAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            Logger.LogError("Exception while trying to save to storage exceptionMessage={ExceptionMessage}", e.Message);
            throw;
        }
        Logger.LogDebug("Successfully saved to the {Storage}", nameof(IStorage));
    }
}

public abstract class StorageEntity
{
}

public interface IDomainConverter<in TDomain, out TEntity> where TDomain : DomainObject
{
    TEntity ToEntity(TDomain domain);
}

public interface IStorage
{
    Task WriteAsync(StorageEntity storageEntity, CancellationToken cancellationToken);
}