using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal abstract class KrakenRequest
{
    [JsonIgnore] 
    public abstract HttpMethod HttpMethod { get; }
    
    [JsonIgnore] 
    public abstract string Pathname { get; }
}