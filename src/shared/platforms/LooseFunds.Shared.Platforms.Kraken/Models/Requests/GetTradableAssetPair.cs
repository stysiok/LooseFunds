using System.Collections.ObjectModel;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetTradableAssetPair : PublicKrakenRequest
{
    public GetTradableAssetPair(IList<Pair> pairs) : base("AssetPairs")
    {
        Pairs = new ReadOnlyCollection<Pair>(pairs);

        new GetTradableAssetPairRequestValidator().ValidateAndThrow(this);
    }

    [JsonProperty("pair")] public IReadOnlyCollection<Pair> Pairs { get; }
}