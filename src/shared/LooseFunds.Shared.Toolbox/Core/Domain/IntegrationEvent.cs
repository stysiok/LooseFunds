using System.Text.Json.Serialization;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using MediatR;

namespace LooseFunds.Shared.Toolbox.Core.Domain;

public abstract record IntegrationEvent : INotification
{
    [JsonIgnore] public abstract IEnumerable<Recipient> Recipients { get; }
}
