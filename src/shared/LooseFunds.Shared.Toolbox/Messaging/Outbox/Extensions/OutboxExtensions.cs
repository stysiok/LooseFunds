using LooseFunds.Shared.Toolbox.Messaging.Models;

namespace LooseFunds.Shared.Toolbox.Messaging.Outbox.Extensions;

internal static class OutboxExtensions
{
    internal static PublishMessage ToPublishMessage(this Models.Outbox outbox) =>
        new(outbox.Id, outbox.Recipient, outbox.Type, outbox.Message);
}
