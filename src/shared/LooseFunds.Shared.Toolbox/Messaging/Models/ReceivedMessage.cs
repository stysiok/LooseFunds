using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging;

public sealed record ReceivedMessage<TContent>
    (Recipient Recipient, Func<TContent, Task> Handler) : MessageBase(Recipient) where TContent : IMessageContent;