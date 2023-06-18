using System.Collections.ObjectModel;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetAssetInfo : PublicKrakenRequest
{
    public GetAssetInfo(IList<Asset> assets) : base("Assets")
    {
        Assets = new ReadOnlyCollection<Asset>(assets);

        new GetAssetInfoRequestValidator().ValidateAndThrow(this);
    }

    [JsonProperty("asset")] public IReadOnlyCollection<Asset> Assets { get; }
}