namespace LooseFunds.Shared.Contracts.Investor.Commands;

using Toolbox.Core.Domain;
using Toolbox.Messaging.Models;

public sealed record CreateInvestmentCommand : IntegrationEvent
{
    public override IEnumerable<Recipient> Recipients { get; } = [Recipient.Investor];
}