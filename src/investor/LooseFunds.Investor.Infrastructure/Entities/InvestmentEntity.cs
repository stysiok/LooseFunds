using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Investor.Infrastructure.Entities;

public sealed class InvestmentEntity : DocumentEntity
{
    public decimal Budget { get; init; }
    public CryptocurrencyEntity[] AvailableCryptocurrencies { get; init; } = null!;
    public CryptocurrencyEntity[] AffordableCryptocurrencies { get; init; } = null!;
    public CryptocurrencyEntity Picked { get; init; } = null!;
    public string TransactionId { get; init; } = null!;
}

public sealed record CryptocurrencyEntity(string Coin, decimal Price, decimal MinimalFraction);