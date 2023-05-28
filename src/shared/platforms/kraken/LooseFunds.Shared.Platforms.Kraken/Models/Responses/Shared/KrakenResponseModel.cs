using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;

internal class KrakenResponse<T> where T : IKrakenResponse
{
    [JsonProperty("errors")]
    public string[]? Errors { get; init; }
    
    [JsonProperty("result")]
    public T? Data { get; init; }
}