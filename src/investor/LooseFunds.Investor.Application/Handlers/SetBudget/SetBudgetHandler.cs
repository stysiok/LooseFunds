using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.SetBudget;

internal sealed class SetBudgetHandler : INotificationHandler<InvestmentCreated>
{
    private readonly IBudgetService _budgetService;
    private readonly ILogger<SetBudgetHandler> _logger;
    private readonly IInvestmentRepository _investmentRepository;

    public SetBudgetHandler(IInvestmentRepository investmentRepository, IBudgetService budgetService,
        ILogger<SetBudgetHandler> logger)
    {
        _investmentRepository = investmentRepository;
        _budgetService = budgetService;
        _logger = logger;
    }

    public async Task Handle(InvestmentCreated notification, CancellationToken cancellationToken)
    {
        var investment = await _investmentRepository.GetAsync(notification.Id, cancellationToken);
        var budget = await _budgetService.GetBudgetAsync(cancellationToken);

        investment.SetBudget(budget);
        _logger.LogInformation("Set {Property} on {Object} [id={Id}, budget={Budget}]", nameof(investment.Budget),
            nameof(Investment), investment.Id, investment.Budget.Amount);
    }
}