using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Notifier.Application.Subscribers;

internal sealed class InvestmentCreatedEventSubscriber : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<InvestmentCreatedEventSubscriber> _logger;

    public InvestmentCreatedEventSubscriber(IMessageSubscriber messageSubscriber,
        ILogger<InvestmentCreatedEventSubscriber> logger)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeAsync<InvestmentCreatedEvent>(Recipient.Investor,
            command =>
            {
                _logger.LogInformation("Message received [message_type={MessageType}]", command.GetType().Name);
            }, stoppingToken);

        return Task.CompletedTask;
    }
}