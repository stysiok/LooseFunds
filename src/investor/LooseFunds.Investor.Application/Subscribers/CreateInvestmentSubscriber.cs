using LooseFunds.Shared.Contracts.Investor;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Messaging.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Subscribers;

internal sealed class CreateInvestmentSubscriber : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<CreateInvestmentSubscriber> _logger;

    public CreateInvestmentSubscriber(IMessageSubscriber messageSubscriber, ILogger<CreateInvestmentSubscriber> logger)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.SubscribeAsync<CreateInvestmentCommand>(Recipient.Investor,
            command =>
            {
                _logger.LogInformation("Message received [message_type={MessageType}]", command.GetType().Name);
                return Task.CompletedTask;
            }, stoppingToken);

        return Task.CompletedTask;
    }
}