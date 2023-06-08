using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record Time(
    [property: JsonProperty("unixtime")] long? Timestamp,
    [property: JsonProperty("rfc1123")] string? Date);