using System.Text.Json;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Consts;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;

internal sealed class Outbox : DomainObject
{
    public Status Status { get; private set; }
    public string Message { get; }
    public string Type { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; } //TODO is it needed
    public Recipient Recipient { get; }

    private Outbox(Guid id, Status status, Recipient recipient, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        Status = status;
        Recipient = recipient;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private Outbox(Guid id, Status status, IntegrationEvent message, Recipient recipient, DateTime createdAt,
        DateTime updatedAt) : this(id, status, recipient, createdAt, updatedAt)
    {
        Type = message.GetType().Name;
        Message = JsonSerializer.Serialize(message, message.GetType());
    }

    private Outbox(Guid id, Status status, string type, string message, Recipient recipient, DateTime createdAt,
        DateTime updatedAt) : this(id, status, recipient, createdAt, updatedAt)
    {
        Type = type;
        Message = message;
    }

    internal static IEnumerable<Outbox> Create(IntegrationEvent integrationEvent)
    {
        var now = DateTime.UtcNow;
        return integrationEvent.Recipients
            .Select(recipient => new Outbox(Guid.NewGuid(), Status.Pending, integrationEvent, recipient, now, now));
    }

    internal static Outbox Restore(Guid id, Status status, string type, string message, Recipient recipient,
        DateTime createdAt, DateTime updatedAt) => new(id, status, type, message, recipient, createdAt, updatedAt);

    internal void MarkAsSent()
    {
        //TODO validate state
        Status = Status.Sent;
    }
}
