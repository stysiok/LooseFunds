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
        amount = Math.Round(amount, 2, MidpointRounding.ToEven);
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

    public static Money operator *(Money money, decimal fraction)
        => new Money(money.Amount * fraction);

    public static bool operator <=(Money left, Money right)
        => left.AmountInPennies <= right.AmountInPennies;

    public static bool operator >=(Money left, Money right)
        => left.AmountInPennies >= right.AmountInPennies;
}

internal sealed class MoneyValidator : AbstractValidator<Money>
{
    public MoneyValidator()
    {
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(decimal.Zero);
        RuleFor(x => x.AmountInPennies).GreaterThanOrEqualTo((uint)0);
    }
}