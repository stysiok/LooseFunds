using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class MarketDataService : IMarketDataService
{
    private readonly IKrakenHttpClient _client;

    public MarketDataService(IKrakenHttpClient client)
    {
        _client = client;
    }

    public Task<GetTime> GetTimeAsync(CancellationToken cancellationToken)
        => _client.SendAsync<Models.Requests.GetTime, GetTime>(new(), cancellationToken);
}