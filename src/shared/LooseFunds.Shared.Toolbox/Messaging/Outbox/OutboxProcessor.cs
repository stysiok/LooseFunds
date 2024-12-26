using LooseFunds.Shared.Toolbox.Messaging.Outbox.Extensions;
using Quartz;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal sealed class OutboxProcessor : IJob
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IMessagePublisher _messagePublisher;

    public OutboxProcessor(IOutboxRepository outboxRepository, IMessagePublisher messagePublisher)
    {
        _outboxRepository = outboxRepository;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute(IJobExecutionContext context) => await SendPendingAsync(context.CancellationToken);

    private async Task SendPendingAsync(CancellationToken cancellationToken)
    {
        var pending = await _outboxRepository.GetAllPendingAsync(cancellationToken);

        foreach (var outbox in pending)
        {
            var message = outbox.ToPublishMessage();
            await _messagePublisher.PublishAsync(message, cancellationToken);
            outbox.MarkAsSent();
            _outboxRepository.Add(outbox);
        }
    }

}