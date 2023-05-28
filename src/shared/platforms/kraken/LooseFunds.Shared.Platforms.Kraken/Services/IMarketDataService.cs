using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IMarketDataService
{
    Task<GetTime> GetTimeAsync(CancellationToken cancellationToken);
}