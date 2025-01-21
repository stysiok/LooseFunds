namespace LooseFunds.Shared.Toolbox.Core.Domain;

public interface IEventsMapper
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}
