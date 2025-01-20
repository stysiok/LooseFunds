using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Contracts.Investor.Events;

public sealed record InvestmentFinishedEvent(string Currency, decimal Price, decimal Budget) : IntegrationEvent
{
    public override Recipient[] Recipients { get; } = [Recipient.Notifier];
}