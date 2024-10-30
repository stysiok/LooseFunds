using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class GetAssetInfoRequestValidator : AbstractValidator<GetAssetInfo>
{
    public GetAssetInfoRequestValidator()
    {
        RuleFor(x => x.Assets).NotEmpty();
    }
}