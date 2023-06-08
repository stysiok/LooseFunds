using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetTickerInformation(
    [property: JsonProperty("pair")] IReadOnlyCollection<Pair> Pairs) : PublicKrakenRequest("Ticker");