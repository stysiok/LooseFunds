namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal interface IOutboxRepository
{
    void Add(Models.Outbox outbox);
    Task<IReadOnlyCollection<Models.Outbox>> GetAllPendingAsync(CancellationToken cancellationToken);
}
