using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Settings;

internal sealed class KrakenCredentialsValidator : AbstractValidator<KrakenCredentials>
{
    public KrakenCredentialsValidator()
    {
        RuleFor(c => c.Key).NotEmpty();
        RuleFor(c => c.Secret).NotEmpty();
    }
}