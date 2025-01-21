namespace LooseFunds.Shared.Toolbox.Core.Domain;

using System.Text.Json.Serialization;
using Messaging.Models;

public abstract record IntegrationEvent
{
    [JsonIgnore] public abstract IEnumerable<Recipient> Recipients { get; }
}
