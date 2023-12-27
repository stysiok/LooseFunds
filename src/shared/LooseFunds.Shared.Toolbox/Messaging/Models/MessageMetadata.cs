namespace LooseFunds.Shared.Toolbox.Messaging.Models;

internal sealed record MessageMetadata(string CorrelationId, DateTime SentOn, Guid MessageId, string MessageType)
{
    public static MessageMetadata Generate(string correlationId, string messageType) =>
        new(correlationId, DateTime.UtcNow, Guid.NewGuid(), messageType);
}