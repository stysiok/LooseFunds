using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class PickedIsNotSet : BusinessRule
{
    private readonly Cryptocurrency? _cryptocurrency;

    public override string ErrorMessage =>
        $"Cryptocurrency value should be null, but was already set to {_cryptocurrency!.Coin}";

    public PickedIsNotSet(Cryptocurrency? cryptocurrency)
    {
        _cryptocurrency = cryptocurrency;
    }

    public override bool Validate() => _cryptocurrency is null;
}