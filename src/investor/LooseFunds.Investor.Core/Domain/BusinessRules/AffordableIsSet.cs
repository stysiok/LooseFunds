using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class AffordableIsSet : BusinessRule
{
    private readonly IImmutableList<Cryptocurrency>? _affordable;
    public override string ErrorMessage => "Affordable cryptocurrencies should be a list, but was null";

    public AffordableIsSet(IImmutableList<Cryptocurrency>? affordable)
    {
        _affordable = affordable;
    }

    public override bool Validate() => _affordable is not null;
}