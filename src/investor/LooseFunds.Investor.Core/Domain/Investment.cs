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
    
    public Money Budget { get; private set; }
    public IImmutableList<Cryptocurrency> Available { get; private set; }
    public IImmutableList<Cryptocurrency> Affordable { get; private set; }
    public Cryptocurrency Picked { get; }

    public static Investment Create()
        => new(Guid.NewGuid());

    public void SetBudget(Money budget)
    {
        Budget = budget;

        AddDomainEvent(new BudgetSet(Id));
    }

    public void SetCryptocurrencies(IImmutableList<Cryptocurrency> cryptocurrencies)
    {
        Available = ImmutableList.CreateRange(cryptocurrencies);
        Affordable = cryptocurrencies.Where(c => c.MinimalFractionPrice <= Budget).ToImmutableList();

        AddDomainEvent(new CryptocurrenciesSet(Id));
    }
}