using LooseFunds.Shared.Toolbox.Messaging.Outbox.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal sealed class OutboxProcessor : IJob
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IOutboxRepository outboxRepository, IMessagePublisher messagePublisher,
        ILogger<OutboxProcessor> logger)
    {
        _outboxRepository = outboxRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context) => await SendPendingAsync(context.CancellationToken);

    private async Task SendPendingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started sending pending messages");
        var pending = await _outboxRepository.GetAllPendingAsync(cancellationToken);
        
        foreach (var outbox in pending)
        {
            var message = outbox.ToPublishMessage();
            await _messagePublisher.PublishAsync(message, cancellationToken);
            outbox.MarkAsSent();
            _outboxRepository.Add(outbox);
        }

        _logger.LogInformation("Finished sending pending messages");
    }

}