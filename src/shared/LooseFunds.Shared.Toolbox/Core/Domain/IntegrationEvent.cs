using System.Text.Json.Serialization;
using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Core.Domain;

public abstract record IntegrationEvent
{
    [JsonIgnore] public abstract IEnumerable<Recipient> Recipients { get; }
}
