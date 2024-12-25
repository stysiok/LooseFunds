namespace LooseFunds.Shared.Toolbox.Messaging.Models;

public sealed record PublishMessage(Guid Id, Recipient Recipient, string Type, string Message)
    : MessageBase(Id, Recipient, Type, Message);