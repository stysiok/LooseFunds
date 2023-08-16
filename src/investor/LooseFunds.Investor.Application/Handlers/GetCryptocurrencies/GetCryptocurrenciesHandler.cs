using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.GetCryptocurrencies;

internal sealed class GetCryptocurrenciesHandler : INotificationHandler<BudgetSet>
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly ICryptocurrencyService _cryptocurrencyService;
    private readonly ILogger<GetCryptocurrenciesHandler> _logger;

    public GetCryptocurrenciesHandler(IInvestmentRepository investmentRepository,
        ICryptocurrencyService cryptocurrencyService, ILogger<GetCryptocurrenciesHandler> logger)
    {
        _investmentRepository = investmentRepository;
        _cryptocurrencyService = cryptocurrencyService;
        _logger = logger;
    }

    public async Task Handle(BudgetSet notification, CancellationToken cancellationToken)
    {
        var investment = await _investmentRepository.GetAsync(notification.Id, cancellationToken);
        var cryptocurrencies = await _cryptocurrencyService.GetCryptocurrenciesAsync(cancellationToken);

        investment.SetCryptocurrencies(cryptocurrencies);
        _logger.LogInformation("Set {Property} on {Object} [id={Id}]", nameof(cryptocurrencies), nameof(Investment),
            investment.Id);
    }
}