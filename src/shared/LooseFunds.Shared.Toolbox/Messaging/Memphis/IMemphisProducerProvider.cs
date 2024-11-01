using LooseFunds.Shared.Toolbox.Messaging.Models;
using Memphis.Client.Producer;

namespace LooseFunds.Shared.Toolbox.Messaging.Memphis;

internal interface IMemphisProducerProvider
{
    Task<MemphisProducer> GetProducerAsync(Recipient recipient, CancellationToken cancellationToken);
}