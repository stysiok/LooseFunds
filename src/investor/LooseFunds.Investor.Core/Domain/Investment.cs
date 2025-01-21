using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.BusinessRules;
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

    public Money? Budget { get; private set; }
    public IImmutableList<Cryptocurrency>? Available { get; private set; }
    public IImmutableList<Cryptocurrency>? Affordable { get; private set; }
    public Cryptocurrency? Picked { get; private set; }
    public string? TransactionId { get; private set; }

    public static Investment Create()
        => new(Guid.NewGuid());

    public void SetBudget(Money budget)
    {
        CheckFor(new BudgetIsNotSet(Budget));
        Budget = budget;

        AddDomainEvent(new BudgetSet(Id));
    }

    public void SetCryptocurrencies(IImmutableList<Cryptocurrency> cryptocurrencies)
    {
        CheckFor(new BudgetIsSet(Budget));
        CheckFor(new AffordableIsNotSet(Affordable));
        CheckFor(new AvailableIsNotSet(Available));

        Available = ImmutableList.CreateRange(cryptocurrencies);
        Affordable = cryptocurrencies.Where(c => c.MinimalFractionPrice <= Budget).ToImmutableList();

        AddDomainEvent(new CryptocurrenciesSet(Id));
    }

    public void PickCryptocurrency()
    {
        CheckFor(new BudgetIsSet(Budget));
        CheckFor(new AffordableIsSet(Affordable));
        CheckFor(new PickedIsNotSet(Picked));

        if (Affordable!.Count == 0) AddDomainEvent(new NoAffordableCryptocurrency(Id));

        int picked = Random.Shared.Next(0, Affordable.Count - 1);
        Picked = Affordable[picked];
        AddDomainEvent(new CryptocurrencyPicked(Id));
    }

    public void SetTransactionId(string? transactionId)
    {
        CheckFor(new BudgetIsSet(Budget));
        CheckFor(new AffordableIsSet(Affordable));
        CheckFor(new PickedIsSet(Picked));
        CheckFor(new TransactionIdIsValid(transactionId));

        TransactionId = transactionId;
        AddDomainEvent(new InvestmentFinished(Picked!, Budget!));
    }
}
