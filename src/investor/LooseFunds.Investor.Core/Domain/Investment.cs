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
    public Cryptocurrency Picked { get; private set; }
    public string TransactionId { get; private set; }

    public static Investment Create()
        => new(Guid.NewGuid());

    public void SetBudget(Money budget)
    {
        //TODO add business rules
        Budget = budget;

        AddDomainEvent(new BudgetSet(Id));
    }

    public void SetCryptocurrencies(IImmutableList<Cryptocurrency> cryptocurrencies)
    {
        //TODO add business rules
        Available = ImmutableList.CreateRange(cryptocurrencies);
        Affordable = cryptocurrencies.Where(c => c.MinimalFractionPrice <= Budget).ToImmutableList();

        AddDomainEvent(new CryptocurrenciesSet(Id));
    }

    public void PickCryptocurrency()
    {
        //TODO add business rules
        if (Affordable.Any() is false) AddDomainEvent(new NoAffordableCryptocurrency(Id));

        var picked = Random.Shared.Next(0, Affordable.Count - 1);
        Picked = Affordable[picked];
        AddDomainEvent(new CryptocurrencyPicked(Id));
    }

    public void SetTransactionId(string transactionId)
    {
        //TODO add validation
        TransactionId = transactionId;
    }
}