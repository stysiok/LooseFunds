using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IUserDataService
{
    Task<GetAccountBalance> GetAccountBalanceAsync(CancellationToken cancellationToken);
}