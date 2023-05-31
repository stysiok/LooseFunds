using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract record KrakenRequest
{
    [JsonIgnore]
    public abstract HttpMethod HttpMethod { get; }

    [JsonIgnore]
    public abstract string Pathname { get; }
}