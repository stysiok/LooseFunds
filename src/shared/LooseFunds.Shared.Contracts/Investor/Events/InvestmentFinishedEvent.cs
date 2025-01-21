namespace LooseFunds.Shared.Contracts.Investor.Events;

using Toolbox.Core.Domain;
using Toolbox.Messaging.Models;

public sealed record InvestmentFinishedEvent(string Currency, decimal Price, decimal Budget) : IntegrationEvent
{
    public override IEnumerable<Recipient> Recipients { get; } = [Recipient.Notifier];
}
