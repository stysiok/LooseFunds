using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain;

public sealed class Investment : DomainObject
{
    private Investment(Guid id) : base(id)
    {
        AddDomainEvent(new InvestmentCreated(id));
    }

    public DateTime Created { get; }
    public Money Budget { get; private set; }
    public ImmutableArray<Cryptocurrency> Available { get; }
    public ImmutableArray<Cryptocurrency> Affordable { get; }
    public Cryptocurrency Picked { get; }

    public static Investment Create()
        => new(Guid.NewGuid());

    public void SetBudget(Money budget)
    {
        Budget = budget;
    }
}