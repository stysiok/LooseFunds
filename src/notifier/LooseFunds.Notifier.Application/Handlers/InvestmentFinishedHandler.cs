using LooseFunds.Shared.Contracts.Investor.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Notifier.Application.Handlers;

internal sealed class InvestmentFinishedHandler : INotificationHandler<InvestmentFinishedEvent>
{
    private readonly ILogger<InvestmentFinishedHandler> _logger;

    public InvestmentFinishedHandler(ILogger<InvestmentFinishedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(InvestmentFinishedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Investment finished");

        return Task.CompletedTask;
    }
}
