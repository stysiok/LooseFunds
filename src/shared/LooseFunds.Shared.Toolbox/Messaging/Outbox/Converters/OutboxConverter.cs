using System.Text.Json;
using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.Messaging.Outbox.Models;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Converters;

internal sealed class OutboxConverter : IDomainObjectConverter<Models.Outbox, OutboxEntity>,
    IEntityObjectConverter<OutboxEntity, Models.Outbox>
{
    public OutboxEntity ToDocumentEntity(Models.Outbox domain)
        => new()
        {
            Id = domain.Id,
            Status = domain.Status,
            Type = domain.Type.ToString(),
            Recipient = domain.Recipient,
            Message = JsonSerializer.Serialize(domain.Message),
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };

    public Models.Outbox ToDomainObject(OutboxEntity entity)
    {
        var message =
            JsonSerializer.Deserialize<IntegrationEvent>(entity.Message) ??
            throw new Exception(); //TODO custom exception
        return Models.Outbox.Restore(entity.Id, entity.Status, message, entity.Recipient, entity.CreatedAt,
            entity.UpdatedAt);
    }

    public DocumentEntity ToDocumentEntity(DomainObject domain) => ToDocumentEntity((Models.Outbox)domain);
}