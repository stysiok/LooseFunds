using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.CreateInvestment;

public sealed record CreateInvestment : INotification;

internal sealed record CreateInvestmentHandler : INotificationHandler<CreateInvestment>
{
    private readonly ILogger<CreateInvestmentHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvestmentHandler(ILogger<CreateInvestmentHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public Task Handle(CreateInvestment notification, CancellationToken cancellationToken)
    {
        var investment = Investment.Create();
        _logger.LogInformation("{Class} object create [id={Id}]",nameof(Investment), investment.Id);
        
        _unitOfWork.Add(investment);
        
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