using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Contracts.Investor;

public sealed record CreateInvestmentCommand : IMessageContent
{
    public static PublishMessage<CreateInvestmentCommand> BuildMessage()
        => new(Recipient.Investor, new CreateInvestmentCommand());
}