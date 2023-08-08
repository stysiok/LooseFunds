using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record AssetPair([property: JsonProperty("ordermin")] string MinimumOrder);