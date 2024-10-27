using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class PickedIsSet : BusinessRule
{
    private readonly Cryptocurrency? _cryptocurrency;
    public override string ErrorMessage => "Cryptocurrency value should be set, but was null";

    public PickedIsSet(Cryptocurrency? cryptocurrency)
    {
        _cryptocurrency = cryptocurrency;
    }

    public override bool Validate() => _cryptocurrency is not null;
}