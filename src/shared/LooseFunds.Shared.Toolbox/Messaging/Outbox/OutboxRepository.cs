using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Repository;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Consts;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Marten;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal sealed class OutboxRepository : RepositoryBase<Models.Outbox, OutboxEntity>, IOutboxRepository, IOutboxStore
{
    private readonly IUnitOfWork _unitOfWork;

    public OutboxRepository(IQuerySession querySession,
        IDomainObjectConverter<Models.Outbox, OutboxEntity> domainConverter,
        IEntityObjectConverter<OutboxEntity, Models.Outbox> entityConverter, IUnitOfWork unitOfWork,
        ILogger<OutboxRepository> logger) : base(querySession, domainConverter, entityConverter, unitOfWork, logger)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        foreach (Models.Outbox outbox in Models.Outbox.Create(integrationEvent))
        {
            Track(outbox);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public void Add(Models.Outbox outbox) => Track(outbox);

    public async Task<IReadOnlyCollection<Models.Outbox>> GetAllPendingAsync(CancellationToken cancellationToken)
    {
        var entities = await QuerySession
            .Query<OutboxEntity>()
            .Where(e => e.Status == Status.Pending)
            .ToListAsync(cancellationToken);

        Logger.LogDebug("Found pending messages [count={Count}]", entities.Count);

        return entities.Select(EntityConverter.ToDomainObject).ToList().AsReadOnly();
    }
}
