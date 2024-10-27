using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class BudgetIsSet : BusinessRule
{
    private readonly Money? _money;
    public override string ErrorMessage => "Budget value should be set, but was null";

    public BudgetIsSet(Money? money)
    {
        _money = money;
    }

    public override bool Validate() => _money is not null;
}