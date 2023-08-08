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
            BudgetInPennies = domain.Budget.AmountInPennies,
            AvailableCryptocurrencies = domain.Available.Select(a =>
                new CryptocurrencyEntity(a.Coin.ToString(), a.Price.AmountInPennies, a.MinimalFraction)).ToArray(),
            AffordableCryptocurrencies = domain.Affordable.Select(a =>
                new CryptocurrencyEntity(a.Coin.ToString(), a.Price.AmountInPennies, a.MinimalFraction)).ToArray()
        };
}