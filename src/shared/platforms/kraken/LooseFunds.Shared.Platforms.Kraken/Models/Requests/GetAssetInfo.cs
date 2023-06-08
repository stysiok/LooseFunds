using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetAssetInfo
    ([property: JsonProperty("asset")] IReadOnlyCollection<Asset> Assets) : PublicKrakenRequest(nameof(GetAssetInfo));