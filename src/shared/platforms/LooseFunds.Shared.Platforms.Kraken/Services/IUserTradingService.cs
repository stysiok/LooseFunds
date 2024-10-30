using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IUserTradingService
{
    Task<Order> AddOrderAsync(Pair pair, decimal volume, CancellationToken cancellationToken);
}