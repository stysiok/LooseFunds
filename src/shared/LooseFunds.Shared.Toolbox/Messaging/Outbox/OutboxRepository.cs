using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Repository;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Consts;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Marten;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal sealed class OutboxRepository : RepositoryBase<Models.Outbox, OutboxEntity>, IOutboxRepository, IOutboxService
{
    public OutboxRepository(IQuerySession querySession,
        IDomainObjectConverter<Models.Outbox, OutboxEntity> domainConverter,
        IEntityObjectConverter<OutboxEntity, Models.Outbox> entityConverter, IUnitOfWork unitOfWork,
        ILogger<OutboxRepository> logger) : base(querySession, domainConverter, entityConverter, unitOfWork, logger)
    {
    }

    public void Add(IntegrationEvent integrationEvent)
    {
        var outboxes = Models.Outbox.Create(integrationEvent);
        foreach (var outbox in outboxes) Track(outbox);
    }

    void IOutboxService.Add(IntegrationEvent integrationEvent)
    {
    }

    public void Add(Models.Outbox outbox) => Track(outbox);

    public async Task<IReadOnlyCollection<Models.Outbox>> GetAllPendingAsync(CancellationToken cancellationToken)
    {
        var entities = await QuerySession
            .Query<OutboxEntity>()
            .Where(e => e.Status == Status.Pending)
            .ToListAsync(cancellationToken);

        return entities.Select(EntityConverter.ToDomainObject).ToList().AsReadOnly();
    }
}