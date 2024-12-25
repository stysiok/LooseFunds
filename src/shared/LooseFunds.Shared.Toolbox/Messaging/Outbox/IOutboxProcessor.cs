namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal interface IOutboxProcessor
{
    Task SendPendingAsync(CancellationToken cancellationToken);
}