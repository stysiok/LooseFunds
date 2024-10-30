namespace LooseFunds.Shared.Platforms.Kraken.Services;

public interface IUserDataService
{
    Task<IReadOnlyDictionary<string, decimal>> GetAccountBalanceAsync(CancellationToken cancellationToken);
}