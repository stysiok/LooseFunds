using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Responses;

public sealed class GetAssetInfo
{
        [JsonProperty("aclass")] public string AssetClass { get; init; } = null!;
        [JsonProperty("altname")] public string AlternateName { get; init; } = null!;
        [JsonProperty("decimals")] public int RecordDecimalPlaces  { get; init; }
        [JsonProperty("display_decimals")] public int OutputDecimalPlaces { get; init; }
        [JsonProperty("collateral_value")] public decimal MarginCollateral { get; init; }
        [JsonProperty("status")] public AssetStatus Status { get; init; }
}