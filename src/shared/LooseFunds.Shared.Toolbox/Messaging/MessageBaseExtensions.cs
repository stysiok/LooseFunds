using System.Text;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Toolbox.Messaging;

internal static class MessageBaseExtensions
{
    public static byte[] ToBytes(this MessageBase messageBase)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBase));
}