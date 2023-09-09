using Memphis.Client.Producer;

namespace LooseFunds.Shared.Toolbox.Messaging;

internal interface IMemphisProducerProvider
{
    Task<MemphisProducer> GetProducerAsync(Recipient recipient, CancellationToken cancellationToken);
}