namespace LooseFunds.Shared.Toolbox.Messaging.Models;

internal sealed class MessageMetadataConsts
{
    public const string CorrelationHeader = "x-correlation-id";
    public const string MessageIdHeader = "x-message-id";
    public const string SentOnUtcHeader = "x-sent-on";
    public const string MessageTypeHeader = "x-message-type";
}