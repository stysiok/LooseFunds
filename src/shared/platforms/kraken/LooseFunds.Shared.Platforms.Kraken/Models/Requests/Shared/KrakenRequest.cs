using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract record KrakenRequest
{
    [JsonIgnore] [InlineParamsIgnore] public readonly string Pathname;

    protected KrakenRequest(string pathname)
    {
        Pathname = pathname;
        new KrakenRequestValidator().ValidateAndThrow(this);
    }
}