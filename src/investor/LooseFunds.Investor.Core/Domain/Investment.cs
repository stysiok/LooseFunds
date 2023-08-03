using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain;

public sealed class Investment : DomainObject
{
    public DateTime Created { get; }
    public Money Budget { get; }
    public ImmutableArray<Cryptocurrency> Available { get; }
    public ImmutableArray<Cryptocurrency> Affordable { get; }
    public Cryptocurrency Picked { get; }

    private Investment(Guid id) : base(id)
    {
        AddDomainEvent(new InvestmentCreated(id));
    }

    public static Investment Create()
        => new(Guid.NewGuid());
}