using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.Storage;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    protected readonly ILogger<UnitOfWork> Logger;

    private readonly HashSet<ITrackable> _commitable = [];
    private readonly IStorage _storage;
    protected readonly HashSet<ITrackable> Tracked = [];
    private bool _storedAlready;

    public UnitOfWork(IStorage storage, ILogger<UnitOfWork> logger)
    {
        _storage = storage;
        Logger = logger;
    }

    public DomainObject? Get(Guid id)
    {
        DomainObject? tracked = Tracked.FirstOrDefault(t => t.Instance.Id.Equals(id))?.Instance;

        if (tracked is null)
        {
            Logger.LogTrace("Did not find any tracked object [id={Id}]", id);
        }
        else
        {
            Logger.LogTrace("Found tracked object [id={Id}]", id);
        }

        return tracked;
    }

    public void Track(ITrackable trackable)
    {
        string name = trackable.Instance.GetType().Name;
        bool wasAdded = Tracked.Add(trackable);
        Persist(trackable);
        if (wasAdded is false)
        {
            Logger.LogError("Domain object {Object} already added to tracking [id={Id}]", name, trackable.Instance.Id);
            return;
        }

        Logger.LogDebug("Domain object {Object} added to tracking", name);
    }

    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_storedAlready)
        {
            Logger.LogTrace("Unit of work has already been commited");
            return;
        }

        List<DocumentEntity> documents = _commitable.Select(trackable => trackable.Convert()).ToList();

        await _storage.StoreAsync(documents, cancellationToken);
        _storedAlready = true;
    }

    protected void Persist(ITrackable trackable) => _commitable.Add(trackable);
}
