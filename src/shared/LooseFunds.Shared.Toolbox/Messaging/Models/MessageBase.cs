namespace LooseFunds.Shared.Toolbox.Messaging.Models;

public abstract record MessageBase(Guid Id, Recipient Recipient, string Type, string Message);