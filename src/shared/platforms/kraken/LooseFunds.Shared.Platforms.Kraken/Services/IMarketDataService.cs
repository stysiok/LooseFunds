using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IMarketDataService
{
    Task<GetTime> GetTimeAsync(CancellationToken cancellationToken);
    Task<IReadOnlyDictionary<string, GetAssetInfo>> GetAssetInfoAsync(IList<Asset> assets,
        CancellationToken cancellationToken);
}