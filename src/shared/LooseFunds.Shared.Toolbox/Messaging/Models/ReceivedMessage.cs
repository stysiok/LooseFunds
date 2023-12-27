namespace LooseFunds.Shared.Toolbox.Messaging.Models;

public sealed record ReceivedMessage<TContent>
    (Recipient Recipient, Func<TContent, Task> Handler) : MessageBase(Recipient) where TContent : IMessageContent;