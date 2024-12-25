using LooseFunds.Shared.Toolbox.Messaging.Models;
using MediatR;

namespace LooseFunds.Shared.Toolbox.Core.Domain;

public interface IDomainEvent : INotification
{
}

public abstract record IntegrationEvent
{
    public abstract Recipient[] Recipients { get; }
}

public interface IEventsMapper
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}