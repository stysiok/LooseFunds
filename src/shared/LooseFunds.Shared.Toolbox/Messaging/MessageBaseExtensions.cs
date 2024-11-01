using System.Text;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Toolbox.Messaging;

internal static class MessageBaseExtensions
{
    public static byte[] ToBytes(this MessageBase messageBase)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBase));
}