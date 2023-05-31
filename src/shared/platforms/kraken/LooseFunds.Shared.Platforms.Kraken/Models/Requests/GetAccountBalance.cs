using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetAccountBalance : PrivateKrakenRequest
{
    public override string Pathname => $"{base.Pathname}Balance";
}