using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal interface IOutboxRepository
{
    void Add(IntegrationEvent integrationEvent);
    void Add(Models.Outbox outbox);
    Task<IReadOnlyCollection<Models.Outbox>> GetAllPendingAsync(CancellationToken cancellationToken);
}

public interface IOutboxService
{
    void Add(IntegrationEvent integrationEvent);
}