namespace LooseFunds.Processor.Application.Options;

internal sealed class PendingOutboxMessagesWorkerOptions
{
    public TimeSpan Delay { get; init; }
}
