using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed record AssetInfo(
    [property: JsonProperty("aclass")] string AssetClass,
    [property: JsonProperty("altname")] string AlternateName,
    [property: JsonProperty("decimals")] int RecordDecimalPlaces,
    [property: JsonProperty("display_decimals")]
    int OutputDecimalPlaces,
    [property: JsonProperty("collateral_value")]
    decimal MarginCollateral,
    [property: JsonProperty("status")] AssetStatus Status);