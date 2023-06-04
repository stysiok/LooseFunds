using LooseFunds.Shared.Platforms.Kraken.Utils;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal record PrivateKrakenRequest : KrakenRequest
{
    [JsonProperty("nonce")] 
    public long Nonce { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 

    public override HttpMethod HttpMethod => HttpMethod.Post;
    
    public override string Pathname => "private/";
}