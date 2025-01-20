using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Consts;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;

public sealed class OutboxEntity : DocumentEntity
{
    public Status Status { get; init; }
    public Recipient Recipient { get; init; }
    public string Type { get; init; }
    public string Message { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}