using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal class PrivateKrakenRequest : KrakenRequest
{
    [JsonProperty("nonce")]
    public long Nonce { get; private init; }

    public PrivateKrakenRequest()
    {
        Nonce = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    
    public override HttpMethod HttpMethod => HttpMethod.Post;
    public override string Pathname => "private/";
}