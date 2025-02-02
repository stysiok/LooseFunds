using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Messaging.RabbitMQ;

public abstract class RabbitMqConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
{
    private readonly IMediator _mediator;
    private readonly ILogger<RabbitMqConsumer<TMessage>> _logger;

    public RabbitMqConsumer(IMediator mediator, ILogger<RabbitMqConsumer<TMessage>> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public virtual Task Consume(ConsumeContext<TMessage> context)
    {
        _mediator.Publish(context.Message, context.CancellationToken);

        return Task.CompletedTask;
    }
}
