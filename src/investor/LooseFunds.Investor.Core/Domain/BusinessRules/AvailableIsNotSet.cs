using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class AvailableIsNotSet : BusinessRule
{
    private readonly IImmutableList<Cryptocurrency>? _available;
    public override string ErrorMessage => "Available cryptocurrencies should be null, but was already set";

    public AvailableIsNotSet(IImmutableList<Cryptocurrency>? available)
    {
        _available = available;
    }

    public override bool Validate() => _available is null;
}