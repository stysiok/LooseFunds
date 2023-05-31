using LooseFunds.Shared.Platforms.Kraken.Utils;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal record PrivateKrakenRequest : KrakenRequest
{
    [JsonProperty("nonce")] 
    public long Nonce { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 

    [InlineParamsIgnore]
    public override HttpMethod HttpMethod => HttpMethod.Post;
    
    [InlineParamsIgnore]
    public override string Pathname => "private/";
}