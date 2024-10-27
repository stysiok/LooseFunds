using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class BudgetIsNotSet : BusinessRule
{
    private readonly Money? _money;
    public override string ErrorMessage => $"Budget value should be null, but was {_money!.Amount}€";

    public BudgetIsNotSet(Money? money)
    {
        _money = money;
    }

    public override bool Validate() => _money is null;
}