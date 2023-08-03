using FluentValidation;

namespace LooseFunds.Investor.Core.Domain.ValueObjects;

public sealed class Money
{
    private Money()
    {
        new MoneyValidator().ValidateAndThrow(this);
    }

    public Money(decimal amount) : this()
    {
        Amount = amount;
        AmountInPennies = (uint)(amount * 100);
    }

    public Money(uint amount) : this()
    {
        Amount = (decimal)(amount / 100.00);
        AmountInPennies = amount;
    }

    public decimal Amount { get; }
    public uint AmountInPennies { get; }

    public static Money operator *(Money money, double fraction)
        => new Money(money.Amount * (decimal)fraction);
}

internal sealed class MoneyValidator : AbstractValidator<Money>
{
    public MoneyValidator()
    {
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(decimal.Zero);
        RuleFor(x => x.AmountInPennies).GreaterThanOrEqualTo((uint)0);
    }
}