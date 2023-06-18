using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class MarketDataService : IMarketDataService
{
    private readonly IKrakenHttpClient _client;

    public MarketDataService(IKrakenHttpClient client)
        => _client = client;

    public Task<Time> GetTimeAsync(CancellationToken cancellationToken)
        => _client.SendAsync<GetTime, Time>(new(), cancellationToken);

    public Task<IReadOnlyDictionary<string, AssetInfo>> GetAssetInfoAsync(IList<Asset> assets,
        CancellationToken cancellationToken)
        => _client.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(new(assets),
            cancellationToken);

    public Task<IReadOnlyDictionary<string, Ticker>> GetTickerInfoAsync(IList<Pair> pairs,
        CancellationToken cancellationToken)
        => _client.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(new(pairs),
            cancellationToken);
}