using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Platforms.Kraken.Services;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

internal sealed class BudgetService : IBudgetService
{
    private const string EuroBalanceKey = "ZEUR";
    private readonly IUserDataService _userDataService;

    public BudgetService(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    public async Task<Money> GetBudgetAsync(CancellationToken cancellationToken)
    {
        var accountBalance = await _userDataService.GetAccountBalanceAsync(cancellationToken);
        var euros = accountBalance[EuroBalanceKey];

        return new Money(euros);
    }
}