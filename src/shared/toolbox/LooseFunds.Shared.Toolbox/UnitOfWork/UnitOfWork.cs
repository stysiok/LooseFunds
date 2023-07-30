using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using Marten;
using MediatR;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDocumentStore _documentStore;
    private readonly IMediator _mediator;
    private readonly HashSet<RepositoryBase> _repositories = new();
    private bool _storedAlready;

    public UnitOfWork(IDocumentStore documentStore, IMediator mediator)
    {
        _documentStore = documentStore;
        _mediator = mediator;
    }

    public void AddRepository(RepositoryBase repository)
    {
        _repositories.Add(repository);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await ProcessDomainEvents(cancellationToken);
        await StoreAsync(cancellationToken);
    }

    private async Task ProcessDomainEvents(CancellationToken cancellationToken)
    {
        foreach (var repository in _repositories)
        {
            var domainEvent = repository.GetNextDomainEvent();
            while (domainEvent is not null)
            {
                await _mediator.Publish(domainEvent, cancellationToken);

                domainEvent = repository.GetNextDomainEvent();
            }
        }
    }

    private async Task StoreAsync(CancellationToken cancellationToken = default)
    {
        if (_storedAlready) return;

        await using var session = _documentStore.LightweightSession();

        var documents = _repositories.SelectMany(r => r.ToDocument());

        session.InsertObjects(documents);
        await session.SaveChangesAsync(cancellationToken);
        _storedAlready = true;
    }
}

public abstract class RepositoryBase
{
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryBase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected HashSet<DomainObject> Tracked { get; } = new();

    protected virtual void Track(DomainObject domainObject)
    {
        Tracked.Add(domainObject);
        _unitOfWork.AddRepository(this);
    }

    protected internal virtual IDomainEvent? GetNextDomainEvent()
        => Tracked.Select(domainObject => domainObject.TryGetNextDomainEvent()).FirstOrDefault();

    protected internal abstract IEnumerable<Entity> ToDocument();
}