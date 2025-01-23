using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Converters;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using LooseFunds.Shared.Toolbox.Storage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal sealed class DomainUnitOfWork : UnitOfWork
{
    private readonly IEventsMapper? _eventsMapper;
    private readonly IMediator _mediator;

    public DomainUnitOfWork(IStorage storage, IMediator mediator, ILogger<DomainUnitOfWork> logger,
        IEventsMapper? eventsMapper = null) : base(storage, logger)
    {
        _mediator = mediator;
        _eventsMapper = eventsMapper;
    }

    public override async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (ITrackable tracked in Tracked)
        {
            IDomainEvent? domainEvent = tracked.Instance.TryGetNextDomainEvent();
            while (domainEvent is not null)
            {
                await _mediator.Publish(domainEvent, cancellationToken);

                IntegrationEvent? @event = _eventsMapper?.Map(domainEvent);
                if (@event is not null)
                {
                    OutboxConverter converter = new();
                    foreach (Outbox outbox in Outbox.Create(@event))
                    {
                        Trackable<Outbox, OutboxEntity> trackable = new(outbox, converter);
                        Persist(trackable);
                    }
                }

                domainEvent = tracked.Instance.TryGetNextDomainEvent();
            }
        }

        await base.CommitAsync(cancellationToken);
    }
}
