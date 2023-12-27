namespace LooseFunds.Shared.Toolbox.Messaging.Models;

public sealed record PublishMessage<TContent>(Recipient Recipient, TContent Message) : MessageBase(Recipient)
    where TContent : IMessageContent;