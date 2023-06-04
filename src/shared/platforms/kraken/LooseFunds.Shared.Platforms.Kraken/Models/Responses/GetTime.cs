using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed class GetTime
{
    [JsonProperty("unixtime")] public long? Timestamp { get; init; }
    [JsonProperty("rfc1123")] public string? Date { get; init; }
}