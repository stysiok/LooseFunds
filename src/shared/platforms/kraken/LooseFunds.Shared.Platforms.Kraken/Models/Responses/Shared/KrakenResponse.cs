using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;

internal sealed record KrakenResponse<T>
{
    [JsonProperty("result")] public T? Data { get; init; }

    [JsonProperty("error")] public string[]? Errors { get; init; }
}