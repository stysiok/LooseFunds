using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Contracts.Investor.Commands;

public sealed record CreateInvestmentCommand : IntegrationEvent
{
    public override Recipient[] Recipients { get; } = { Recipient.Investor };
}