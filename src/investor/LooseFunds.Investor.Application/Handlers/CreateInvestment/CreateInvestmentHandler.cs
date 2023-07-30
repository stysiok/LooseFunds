using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.CreateInvestment;

public sealed record CreateInvestment : INotification;

internal sealed record CreateInvestmentHandler : INotificationHandler<CreateInvestment>
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly ILogger<CreateInvestmentHandler> _logger;

    public CreateInvestmentHandler(IInvestmentRepository investmentRepository, ILogger<CreateInvestmentHandler> logger)
    {
        _logger = logger;
        _investmentRepository = investmentRepository;
    }

    public Task Handle(CreateInvestment notification, CancellationToken cancellationToken)
    {
        var investment = Investment.Create();
        _logger.LogInformation("{Class} object create [id={Id}]", nameof(Investment), investment.Id);

        _investmentRepository.Save(investment);

        return Task.CompletedTask;
    }
}

internal sealed class CollectAvailablePairs : INotificationHandler<InvestmentCreated>
{
    private readonly ILogger<CollectAvailablePairs> _logger;

    public CollectAvailablePairs(ILogger<CollectAvailablePairs> logger)
    {
        _logger = logger;
    }

    public Task Handle(InvestmentCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("woop woop 🥳");
        return Task.CompletedTask;
    }
}