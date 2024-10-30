using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record Order(
    [property: JsonProperty("descr")] OrderInformation OrderInformation,
    [property: JsonProperty("txid")] string[] TransactionsIds);