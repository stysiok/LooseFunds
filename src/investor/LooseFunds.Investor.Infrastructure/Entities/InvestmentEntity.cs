using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Investor.Infrastructure.Entities;

public sealed class InvestmentEntity : DocumentEntity
{
    public uint BudgetInPennies { get; init; }

    public CryptocurrencyEntity[] AvailableCryptocurrencies { get; init; }
    public CryptocurrencyEntity[] AffordableCryptocurrencies { get; init; }
}

public sealed record CryptocurrencyEntity(string Coin, uint PriceInPennies, double MinimalFraction);