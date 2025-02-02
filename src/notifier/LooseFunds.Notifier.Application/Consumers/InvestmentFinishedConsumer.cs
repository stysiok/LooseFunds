using LooseFunds.Shared.Contracts.Investor.Events;
using LooseFunds.Shared.Toolbox.Messaging.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Notifier.Application.Consumers;

public sealed class InvestmentFinishedConsumer : RabbitMqConsumer<InvestmentFinishedEvent>
{
    public InvestmentFinishedConsumer(IMediator mediator, ILogger<RabbitMqConsumer<InvestmentFinishedEvent>> logger) :
        base(mediator, logger)
    {
    }
}
