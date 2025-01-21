namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;

using System.Text.Json;
using Consts;
using Core.Domain;
using Messaging.Models;

internal sealed class Outbox : DomainObject
{
    public Status Status { get; private set; }
    public string Message { get; }
    public Type Type { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; } //TODO is it needed
    public Recipient Recipient { get; }

    private Outbox(Guid id, Status status, IntegrationEvent message, Recipient recipient, DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Status = status;
        Type = message.GetType();
        Recipient = recipient;
        this.Message = JsonSerializer.Serialize(message, message.GetType());
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static IEnumerable<Outbox> Create(IntegrationEvent integrationEvent)
    {
        var now = DateTime.UtcNow;
        return integrationEvent.Recipients
            .Select(recipient => new Outbox(Guid.NewGuid(), Status.Pending, integrationEvent, recipient, now, now));
    }

    public static Outbox Restore(Guid id, Status status, IntegrationEvent message, Recipient recipient,
        DateTime createdAt,
        DateTime updatedAt) => new(id, status, message, recipient, createdAt, updatedAt);

    public void MarkAsSent()
    {
        //TODO validate state
        Status = Status.Sent;
    }
}
