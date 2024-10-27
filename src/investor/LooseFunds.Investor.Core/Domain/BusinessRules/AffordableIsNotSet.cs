using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class AffordableIsNotSet : BusinessRule
{
    private readonly IImmutableList<Cryptocurrency>? _affordable;
    public override string ErrorMessage => "Affordable cryptocurrencies should be null, but was already set";

    public AffordableIsNotSet(IImmutableList<Cryptocurrency>? affordable)
    {
        _affordable = affordable;
    }

    public override bool Validate() => _affordable is null;
}