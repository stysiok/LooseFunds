using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Clients;

internal interface IKrakenHttpClient
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) 
        where TRequest : KrakenRequest;
}