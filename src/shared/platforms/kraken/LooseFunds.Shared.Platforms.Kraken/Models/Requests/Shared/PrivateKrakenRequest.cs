using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract record PrivateKrakenRequest : KrakenRequest
{
    protected PrivateKrakenRequest(string method) : base($"{KrakenRequestsConsts.PrivatePrefix}{method}")
    {
        new PrivateKrakenRequestValidator().ValidateAndThrow(this);
    }

    [JsonProperty("nonce")] public long Nonce { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}