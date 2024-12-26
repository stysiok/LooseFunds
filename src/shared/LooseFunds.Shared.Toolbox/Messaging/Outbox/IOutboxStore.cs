using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox;

public interface IOutboxStore
{
    void Add(IntegrationEvent integrationEvent);
}