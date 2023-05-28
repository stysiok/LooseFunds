using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed class GetTime : PublicKrakenRequest
{
    public override string Pathname => $"{base.Pathname}Time";
}