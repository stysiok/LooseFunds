using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;
using LooseFunds.Shared.Platforms.Kraken.Services;

namespace LooseFunds.Investor.Api.E2E.Infrastructure.Fakes;

internal sealed class FakeMarketDataService : IMarketDataService
{
    public Task<Time> GetTimeAsync(CancellationToken cancellationToken)
        => Task.FromResult(new Time(0, "Thu, 01 Jan 1970 00:00:00 GMT"));

    public Task<IReadOnlyDictionary<string, AssetInfo>> GetAssetInfoAsync(IList<Asset> assets,
        CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyDictionary<string, AssetInfo>>(new Dictionary<string, AssetInfo>());

    public Task<IReadOnlyDictionary<string, Ticker>> GetTickerInfoAsync(IList<Pair> pairs,
        CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyDictionary<string, Ticker>>(new Dictionary<string, Ticker>());

    public Task<IReadOnlyDictionary<string, AssetPair>> GetTradableAssetPairAsync(IList<Pair> pairs,
        CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyDictionary<string, AssetPair>>(new Dictionary<string, AssetPair>());
}
