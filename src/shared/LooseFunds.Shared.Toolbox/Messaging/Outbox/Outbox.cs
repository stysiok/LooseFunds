using System.Text.Json;
using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.Core.Repository;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Marten;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

public class Inbox
{
}

internal interface IOutboxService
{
    void Add(IntegrationEvent integrationEvent);
    Task<IReadOnlyCollection<Outbox>> GetAllPendingAsync(CancellationToken cancellationToken);
}

internal enum Status
{
    Pending,
    Sent,
    Failed
}

internal sealed class Outbox : DomainObject
{
    public Status Status { get; }
    public IntegrationEvent Message { get; }
    public Type Type { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public Recipient[] Recipients { get; }

    private Outbox(Guid id, Status status, IntegrationEvent message, DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Status = status;
        Type = message.GetType();
        Recipients = message.Recipients;
        Message = message;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Outbox Create(IntegrationEvent integrationEvent)
    {
        var now = DateTime.UtcNow;
        return new Outbox(Guid.NewGuid(), Status.Pending, integrationEvent, now, now);
    }

    public static Outbox Restore(Guid id, Status status, IntegrationEvent message, DateTime createdAt,
        DateTime updatedAt) => new(id, status, message, createdAt, updatedAt);
}

internal sealed class OutboxEntity : DocumentEntity
{
    public Status Status { get; init; }
    public Recipient[] Recipients { get; init; }
    public string Type { get; init; }
    public string Message { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

internal sealed class OutboxConverter : IDomainObjectConverter<Outbox, OutboxEntity>,
    IEntityObjectConverter<OutboxEntity, Outbox>
{
    public OutboxEntity ToDocumentEntity(Outbox domain)
        => new()
        {
            Id = domain.Id,
            Status = domain.Status,
            Type = domain.Type.ToString(),
            Recipients = domain.Recipients,
            Message = JsonSerializer.Serialize(domain.Message),
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };

    public Outbox ToDomainObject(OutboxEntity entity)
    {
        var message = JsonSerializer.Deserialize<IntegrationEvent>(entity.Message);
        return Outbox.Restore(entity.Id, entity.Status, message, entity.CreatedAt, entity.UpdatedAt);
    }
}

internal sealed class OutboxRepository : RepositoryBase<Outbox, OutboxEntity>, IOutboxService
{
    public OutboxRepository(IQuerySession querySession, IDomainObjectConverter<Outbox, OutboxEntity> domainConverter,
        IEntityObjectConverter<OutboxEntity, Outbox> entityConverter, IUnitOfWork unitOfWork,
        ILogger<OutboxRepository> logger) : base(querySession, domainConverter, entityConverter, unitOfWork, logger)
    {
    }

    public void Add(IntegrationEvent integrationEvent)
    {
        var outbox = Outbox.Create(integrationEvent);
        Track(outbox);
    }

    public async Task<IReadOnlyCollection<Outbox>> GetAllPendingAsync(CancellationToken cancellationToken)
    {
        var entities = await QuerySession
            .Query<OutboxEntity>()
            .Where(e => e.Status == Status.Pending)
            .ToListAsync(cancellationToken);

        return entities.Select(EntityConverter.ToDomainObject).ToList().AsReadOnly();
    }
}

internal static class OutboxExtensions
{
    internal static IEnumerable<PublishMessage> ToPublishMessage(this Outbox outbox)
    {
        var message = JsonSerializer.SerializeToDocument(outbox);

        return outbox.Recipients.Select(recipient => new PublishMessage(recipient, message));
    }
}

internal interface IOutboxProcessor
{
    Task SendPendingAsync(CancellationToken cancellationToken);
}

internal sealed class OutboxProcessor : IOutboxProcessor
{
    private readonly IOutboxService _outboxService;
    private readonly IMessagePublisher _messagePublisher;

    public OutboxProcessor(IOutboxService outboxService, IMessagePublisher messagePublisher)
    {
        _outboxService = outboxService;
        _messagePublisher = messagePublisher;
    }

    public async Task SendPendingAsync(CancellationToken cancellationToken)
    {
        var pending = await _outboxService.GetAllPendingAsync(cancellationToken);

        var messages = pending.SelectMany(p => p.ToPublishMessage());

        foreach (var message in messages) await _messagePublisher.PublishAsync(message, cancellationToken);
    }
}