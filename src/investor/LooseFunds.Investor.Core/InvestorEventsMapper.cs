using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Shared.Contracts.Investor.Events;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core;

internal sealed class InvestorEventsMapper : IEventsMapper
{
    public IntegrationEvent? Map(IDomainEvent domainEvent)
        => domainEvent switch
        {
            InvestmentFinished investmentFinished => new InvestmentFinishedEvent(
                investmentFinished.Picked.Coin.ToString(), investmentFinished.Picked.Price.Amount,
                investmentFinished.Budget.Amount),
            _ => null
        };
}