namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal record PublicKrakenRequest : KrakenRequest
{
    public override HttpMethod HttpMethod { get; } = HttpMethod.Get;
    public override string Pathname => "public/";
}