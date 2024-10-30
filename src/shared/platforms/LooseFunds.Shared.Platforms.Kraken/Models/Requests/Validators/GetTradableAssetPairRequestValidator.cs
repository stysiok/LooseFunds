using FluentValidation;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

internal sealed class GetTradableAssetPairRequestValidator : AbstractValidator<GetTradableAssetPair>
{
    public GetTradableAssetPairRequestValidator()
    {
        RuleFor(x => x.Pairs).NotEmpty();
    }
}