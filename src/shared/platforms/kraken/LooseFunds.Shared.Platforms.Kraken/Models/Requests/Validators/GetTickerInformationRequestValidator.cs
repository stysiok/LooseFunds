using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class GetTickerInformationRequestValidator : AbstractValidator<GetTickerInformation>
{
    public GetTickerInformationRequestValidator()
    {
        RuleFor(x => x.Pairs).NotEmpty();
    }
}