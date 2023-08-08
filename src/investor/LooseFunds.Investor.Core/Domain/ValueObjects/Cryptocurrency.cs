using FluentValidation;
using LooseFunds.Investor.Core.Domain.Consts;

namespace LooseFunds.Investor.Core.Domain.ValueObjects;

public sealed record Cryptocurrency
{
    public Coin Coin { get; }
    public Money Price { get; }
    public double MinimalFraction { get; }
    public Money MinimalFractionPrice { get; }

    public Cryptocurrency(Coin coin, Money price, double minimalFraction)
    {
        Coin = coin;
        Price = price;
        MinimalFraction = minimalFraction;
        MinimalFractionPrice = price * minimalFraction;

        new CryptocurrencyValidator().ValidateAndThrow(this);
    }
}

internal sealed class CryptocurrencyValidator : AbstractValidator<Cryptocurrency>
{
    private const uint MinimalFractionSize = 0;
    
    public CryptocurrencyValidator()
    {
        RuleFor(x => x.MinimalFraction).GreaterThan(MinimalFractionSize);
    }
}