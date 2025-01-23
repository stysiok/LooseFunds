namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

public interface IOutboxProcessor
{
    Task SendPendingAsync(CancellationToken cancellationToken);
}
