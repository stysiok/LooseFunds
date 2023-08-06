using LooseFunds.Investor.Core.Domain.ValueObjects;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

public interface IBudgetService
{
    Task<Money> GetBudgetAsync(CancellationToken cancellationToken);
}