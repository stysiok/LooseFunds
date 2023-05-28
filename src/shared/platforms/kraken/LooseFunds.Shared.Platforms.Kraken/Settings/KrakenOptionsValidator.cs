using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Settings;

internal sealed class KrakenOptionsValidator : AbstractValidator<KrakenOptions>
{
    public KrakenOptionsValidator()
    {
        RuleFor(o => o.Url).NotEmpty();
    }
}