using LooseFunds.Shared.Toolbox.Messaging.Outbox.Extensions;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

internal sealed class OutboxProcessor : IOutboxProcessor
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    // private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IOutboxRepository outboxRepository,
        IUnitOfWork unitOfWork, //IMessagePublisher messagePublisher,
        ILogger<OutboxProcessor> logger)
    {
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
        // _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task SendPendingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started sending pending messages");
        var pending = await _outboxRepository.GetAllPendingAsync(cancellationToken);

        foreach (var outbox in pending)
        {
            var message = outbox.ToPublishMessage();
            // await _messagePublisher.PublishAsync(message, cancellationToken);
            outbox.MarkAsSent();
            _outboxRepository.Add(outbox);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Finished sending pending messages");
    }

}
