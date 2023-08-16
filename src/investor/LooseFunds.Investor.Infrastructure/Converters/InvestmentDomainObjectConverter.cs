using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Shared.Toolbox.Core.Converters;

namespace LooseFunds.Investor.Infrastructure.Converters;

public sealed class InvestmentDomainObjectConverter : IDomainObjectConverter<Investment, InvestmentEntity>
{
    public InvestmentEntity ToDocumentEntity(Investment domain)
        => new()
        {
            Id = domain.Id,
            Budget = domain.Budget.Amount,
            AvailableCryptocurrencies = domain.Available.Select(a =>
                new CryptocurrencyEntity(a.Coin.ToString(), a.Price.Amount, a.MinimalFraction)).ToArray(),
            AffordableCryptocurrencies = domain.Affordable.Select(a =>
                new CryptocurrencyEntity(a.Coin.ToString(), a.Price.Amount, a.MinimalFraction)).ToArray(),
            Picked = new CryptocurrencyEntity(domain.Picked.Coin.ToString(), domain.Picked.Price.Amount,
                domain.Picked.MinimalFraction),
            TransactionId = domain.TransactionId
        };
}