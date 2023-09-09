using LooseFunds.Shared.Contracts.Investor;
using LooseFunds.Shared.Toolbox.Messaging;
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
            }, stoppingToken);

        return Task.CompletedTask;
    }
}