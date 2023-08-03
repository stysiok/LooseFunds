using FluentValidation;
using LooseFunds.Investor.Core.Domain.Consts;

namespace LooseFunds.Investor.Core.Domain.ValueObjects;

public sealed record Cryptocurrency
{
    public Coin Coin { get; }
    public Money Price { get; }
    public double Fraction { get; }
    public Money FractionPrice { get; }

    private Cryptocurrency()
    {
        new CryptocurrencyValidator().ValidateAndThrow(this);
    }
    
    public Cryptocurrency(Coin coin, Money price, double fraction) : this()
    {
        Coin = coin;
        Price = price;
        Fraction = fraction;
        FractionPrice = price * fraction;
    }
}

internal sealed class CryptocurrencyValidator : AbstractValidator<Cryptocurrency>
{
    private const uint MinimalFractionSize = 0;
    
    public CryptocurrencyValidator()
    {
        RuleFor(x => x.Fraction).GreaterThan(MinimalFractionSize);
    }
}