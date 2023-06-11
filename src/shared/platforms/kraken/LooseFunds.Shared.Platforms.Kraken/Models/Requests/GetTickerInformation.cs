using System.Collections.ObjectModel;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetTickerInformation : PublicKrakenRequest
{
    [JsonProperty("pair")] public IReadOnlyCollection<Pair> Pairs { get; }
    
    public GetTickerInformation(IList<Pair> pairs) : base("Ticker")
    {
        Pairs = new ReadOnlyCollection<Pair>(pairs);

        new GetTickerInformationRequestValidator().ValidateAndThrow(this);
    }
}