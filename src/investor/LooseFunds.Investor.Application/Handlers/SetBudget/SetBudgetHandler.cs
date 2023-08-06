using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Repositories;
using MediatR;

namespace LooseFunds.Investor.Application.Handlers.SetBudget;

internal sealed class SetBudgetHandler : INotificationHandler<InvestmentCreated>
{
    private readonly IBudgetService _budgetService;
    private readonly IInvestmentRepository _investmentRepository;

    public SetBudgetHandler(IInvestmentRepository investmentRepository, IBudgetService budgetService)
    {
        _investmentRepository = investmentRepository;
        _budgetService = budgetService;
    }

    public async Task Handle(InvestmentCreated notification, CancellationToken cancellationToken)
    {
        var investment = await _investmentRepository.GetAsync(notification.Id, cancellationToken);
        var budget = await _budgetService.GetBudgetAsync(cancellationToken);

        investment.SetBudget(budget);
    }
}