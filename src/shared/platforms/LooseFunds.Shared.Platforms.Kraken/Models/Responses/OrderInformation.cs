using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record OrderInformation(
    [property: JsonProperty("order")] string Description,
    [property: JsonProperty("close")] string CloseDescription);