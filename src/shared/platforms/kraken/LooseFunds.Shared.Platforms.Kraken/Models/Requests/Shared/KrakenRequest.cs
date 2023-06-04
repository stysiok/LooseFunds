using LooseFunds.Shared.Platforms.Kraken.Utils;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract record KrakenRequest
{
    [JsonIgnore]
    [InlineParamsIgnore]
    public abstract HttpMethod HttpMethod { get; }

    [JsonIgnore]
    [InlineParamsIgnore]
    public abstract string Pathname { get; }
}