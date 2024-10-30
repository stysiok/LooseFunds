using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class KrakenRequestValidator : AbstractValidator<KrakenRequest>
{
    public KrakenRequestValidator()
    {
        RuleFor(x => x.Pathname)
            .NotEmpty()
            .DependentRules(() =>
            {
                RuleFor(x => x.Pathname)
                    .Must(x =>
                        x.StartsWith(KrakenRequestsConsts.PublicPrefix) ||
                        x.StartsWith(KrakenRequestsConsts.PrivatePrefix));
            });
    }
}