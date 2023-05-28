using LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed class GetTime : IKrakenResponse
{
    [JsonProperty("unixtime")] public long? Timestamp { get; set; }

    [JsonProperty("rfc1123")] public string? Date { get; set; }
}