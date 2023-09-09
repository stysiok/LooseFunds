namespace LooseFunds.Shared.Toolbox.Messaging;

public sealed record PublishMessage<TContent>(Recipient Recipient, TContent Message) : MessageBase(Recipient)
    where TContent : IMessageContent;