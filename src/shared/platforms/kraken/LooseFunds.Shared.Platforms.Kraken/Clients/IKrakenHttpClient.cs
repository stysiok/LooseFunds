using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Clients;

internal interface IKrakenHttpClient
{
    Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : PrivateKrakenRequest;

    Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : PublicKrakenRequest;
}