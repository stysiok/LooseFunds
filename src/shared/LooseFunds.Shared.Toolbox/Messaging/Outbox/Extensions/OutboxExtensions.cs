using System.Text.Json;
using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Extensions;

internal static class OutboxExtensions
{
    internal static PublishMessage ToPublishMessage(this Models.Outbox outbox)
    {
        var message = JsonSerializer.Serialize(outbox.Message);
        return new PublishMessage(outbox.Id, outbox.Recipient, outbox.Type.Name, message);
    }
}