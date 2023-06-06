using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed class Ticker
{
    [JsonProperty("a")] public string[] Ask { get; init; } = null!;
    [JsonProperty("b")] public string[] Bid { get; init; } = null!;
    [JsonProperty("c")] public string[] LastTradeClosed { get; init; } = null!;
    [JsonProperty("v")] public string[] Volume { get; init; } = null!;
    [JsonProperty("p")] public string[] AveragePrice { get; init; } = null!;
    [JsonProperty("t")] public int[] TradesNumber { get; init; } = null!;
    [JsonProperty("l")] public string[] Low { get; init; } = null!;
    [JsonProperty("h")] public string[] High { get; init; } = null!;
    [JsonProperty("o")] public string OpeningPrice { get; init; } = null!;
}