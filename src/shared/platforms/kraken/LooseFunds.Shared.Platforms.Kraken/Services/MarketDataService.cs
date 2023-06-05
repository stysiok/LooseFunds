using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class MarketDataService : IMarketDataService
{
    private readonly IKrakenHttpClient _client;

    public MarketDataService(IKrakenHttpClient client)
    {
        _client = client;
    }

    public Task<Time> GetTimeAsync(CancellationToken cancellationToken)
        => _client.GetAsync<Models.Requests.GetTime, Time>(new(), cancellationToken);

    public Task<IReadOnlyDictionary<string, AssetInfo>> GetAssetInfoAsync(IList<Asset> assets,
        CancellationToken cancellationToken)
        => _client.GetAsync<Models.Requests.GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(
            new(new Assets(assets)), cancellationToken);

}