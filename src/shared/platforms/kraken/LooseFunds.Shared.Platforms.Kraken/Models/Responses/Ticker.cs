using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record Ticker(
    [property: JsonProperty("a")] string[] Ask,
    [property: JsonProperty("b")] string[] Bid,
    [property: JsonProperty("c")] string[] LastTradeClosed,
    [property: JsonProperty("v")] string[] Volume,
    [property: JsonProperty("p")] string[] AveragePrice,
    [property: JsonProperty("t")] int[] TradesNumber,
    [property: JsonProperty("l")] string[] Low,
    [property: JsonProperty("h")] string[] High,
    [property: JsonProperty("o")] string OpeningPrice);