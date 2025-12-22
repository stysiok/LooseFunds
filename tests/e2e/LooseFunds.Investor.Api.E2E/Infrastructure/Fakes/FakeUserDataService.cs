using LooseFunds.Shared.Platforms.Kraken.Services;

namespace LooseFunds.Investor.Api.E2E.Infrastructure.Fakes;

internal sealed class FakeUserDataService : IUserDataService
{
    public Task<IReadOnlyDictionary<string, decimal>> GetAccountBalanceAsync(CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyDictionary<string, decimal>>(new Dictionary<string, decimal>());
}
