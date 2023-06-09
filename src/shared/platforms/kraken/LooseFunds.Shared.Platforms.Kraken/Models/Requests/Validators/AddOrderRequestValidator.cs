using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class AddOrderRequestValidator : AbstractValidator<AddOrder>
{
    public AddOrderRequestValidator()
    {
        RuleFor(x => x.Volume).NotNull().GreaterThan(decimal.Zero);
        When(x => x.Price is not null, () => RuleFor(x => x.Price).GreaterThan(0));
    }
}