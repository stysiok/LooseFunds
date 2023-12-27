using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Memphis.Client.Core;
using NATS.Client;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal static class MemphisExtensions
{
    public static TContent ToContent<TContent>(this MemphisMessage message)
        => JsonConvert.DeserializeObject<TContent>(Encoding.UTF8.GetString(message.GetData()))
           ?? throw new Exception(); //TODO: Exception implementation

    public static byte[] ToBytes(this MessageBase messageBase)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBase));

    public static MessageMetadata ToMessageMetadata(this MsgHeader header)
    {
        var correlationId = header[MessageMetadataConsts.CorrelationHeader];
        var sentOnUtc = DateTime.Parse(header[MessageMetadataConsts.SentOnUtcHeader]);
        var messageId = Guid.Parse(header[MessageMetadataConsts.MessageIdHeader]);
        var messageType = header[MessageMetadataConsts.MessageTypeHeader]!;

        return new MessageMetadata(correlationId, sentOnUtc, messageId, messageType);
    }

    public static NameValueCollection ToHeaders(this MessageMetadata messageMetadata)
        => new()
        {
            { MessageMetadataConsts.CorrelationHeader, messageMetadata.CorrelationId },
            { MessageMetadataConsts.MessageIdHeader, messageMetadata.MessageId.ToString() },
            { MessageMetadataConsts.MessageTypeHeader, messageMetadata.MessageType },
            { MessageMetadataConsts.SentOnUtcHeader, messageMetadata.SentOn.ToString(CultureInfo.InvariantCulture) }
        };
}

//TODO: Implement inbox/outbox 