using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;

internal record KrakenResponse<T> where T : IKrakenResponse
{
    [JsonProperty("result")]
    public T? Data { get; init; }
    [JsonProperty("error")]
    public string[]? Errors { get; init; }
}