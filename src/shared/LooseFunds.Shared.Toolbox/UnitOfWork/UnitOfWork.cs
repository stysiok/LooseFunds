using LooseFunds.Shared.Toolbox.Core.Repository;
using Marten;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDocumentStore _documentStore;
    private readonly IMediator _mediator;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly HashSet<IRepositoryBase> _repositories = new();
    private bool _storedAlready;

    public UnitOfWork(IDocumentStore documentStore, IMediator mediator, ILogger<UnitOfWork> logger)
    {
        _documentStore = documentStore;
        _mediator = mediator;
        _logger = logger;
    }

    public void AddRepository(IRepositoryBase repository)
    {
        var repositoryName = repository.GetType().Name;

        var wasAdded = _repositories.Add(repository);
        if (wasAdded is false) _logger.LogError("Repository {Object} was not added to repositories", repositoryName);

        _logger.LogDebug("Repository {Object} added to the repositories", repositoryName);
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

        session.StoreObjects(documents);
        await session.SaveChangesAsync(cancellationToken);
        _storedAlready = true;
    }
}