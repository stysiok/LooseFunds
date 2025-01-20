using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Converters;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using LooseFunds.Shared.Toolbox.Storage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IStorage _storage;
    private readonly IEventsMapper? _eventsMapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UnitOfWork> _logger;
    private readonly HashSet<ITrackable> _tracked = [];
    private bool _storedAlready;

    public UnitOfWork(IStorage storage, IMediator mediator, ILogger<UnitOfWork> logger,
        IEventsMapper? eventsMapper = null)
    {
        _storage = storage;
        _mediator = mediator;
        _logger = logger;
        _eventsMapper = eventsMapper; 
    }

    public DomainObject? Get(Guid id)
    {
        var tracked = _tracked.FirstOrDefault(t => t.Tracked.Id.Equals(id))?.Tracked;

        if (tracked is null)
            _logger.LogTrace("Did not find any tracked object [id={Id}]", id);
        else
            _logger.LogTrace("Found tracked object [id={Id}]", id);

        return tracked;
    }

    public void Track(ITrackable trackable)
    {
        var name = trackable.Tracked.GetType().Name;
        var wasAdded = _tracked.Add(trackable);
        if (wasAdded is false)
        {
            _logger.LogError("Domain object {Object} already added to tracking [id={Id}]", name, trackable.Tracked.Id);
            return;
        }

        _logger.LogDebug("Domain object {Object} added to tracking", name);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var tracked = await ProcessDomainEvents(cancellationToken);
        await StoreAsync(tracked, cancellationToken);
    }

    private async Task<IEnumerable<ITrackable>> ProcessDomainEvents(CancellationToken cancellationToken)
    {
        var tracked = new List<ITrackable>();
        foreach (var trackable in _tracked)
        {
            tracked.Add(trackable);
            var domainEvent = trackable.Tracked.TryGetNextDomainEvent();
            while (domainEvent is not null)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
                var @event = _eventsMapper?.Map(domainEvent);
                if (@event is not null)
                {
                    var outboxes = Outbox.Create(@event);
                    tracked.AddRange(
                        outboxes.Select(o => new Trackable<Outbox, OutboxEntity>(o, new OutboxConverter())));
                    //TODO decorator uow
                }

                domainEvent = trackable.Tracked.TryGetNextDomainEvent();
            }
        }

        return tracked;
    }

    private async Task StoreAsync(IEnumerable<ITrackable> tracked, CancellationToken cancellationToken = default)
    {
        if (_storedAlready) return;

        var documents = tracked.Select(trackable => trackable.Convert()).ToList();

        await _storage.StoreAsync(documents, cancellationToken);
        _storedAlready = true;
    }
}