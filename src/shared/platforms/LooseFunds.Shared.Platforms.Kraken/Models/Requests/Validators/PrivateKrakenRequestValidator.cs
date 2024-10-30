using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class PrivateKrakenRequestValidator : AbstractValidator<PrivateKrakenRequest>
{
    public PrivateKrakenRequestValidator()
    {
        RuleFor(x => x.Nonce).GreaterThan(0);
    }
}