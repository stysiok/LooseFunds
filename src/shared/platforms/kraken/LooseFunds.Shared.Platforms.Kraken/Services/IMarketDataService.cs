using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IMarketDataService
{
    Task<Time> GetTimeAsync(CancellationToken cancellationToken);
    Task<IReadOnlyDictionary<string, AssetInfo>> GetAssetInfoAsync(IList<Asset> assets,
        CancellationToken cancellationToken);
}