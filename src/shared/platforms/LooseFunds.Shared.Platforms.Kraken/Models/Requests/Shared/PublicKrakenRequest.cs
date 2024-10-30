namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract record PublicKrakenRequest : KrakenRequest
{
    protected PublicKrakenRequest(string method) : base($"{KrakenRequestsConsts.PublicPrefix}{method}")
    {
    }
}