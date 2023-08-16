using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Platforms.Kraken.Services;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

internal sealed class BudgetService : IBudgetService
{
    private const string EuroBalanceKey = "ZEUR";
    private readonly IUserDataService _userDataService;
    private readonly ILogger<BudgetService> _logger;

    public BudgetService(IUserDataService userDataService, ILogger<BudgetService> logger)
    {
        _userDataService = userDataService;
        _logger = logger;
    }

    public async Task<Money> GetBudgetAsync(CancellationToken cancellationToken)
    {
        var accountBalance = await _userDataService.GetAccountBalanceAsync(cancellationToken);
        _logger.LogDebug("{Method} got {Object}", nameof(GetBudgetAsync), nameof(accountBalance));
        
        var euros = accountBalance[EuroBalanceKey];
        _logger.LogDebug("Found current € budget in {Object} [budget={Budget}€]", nameof(accountBalance), euros);
        return new Money(euros);
    }
}