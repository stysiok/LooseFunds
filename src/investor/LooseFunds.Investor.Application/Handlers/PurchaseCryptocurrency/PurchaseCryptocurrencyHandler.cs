using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.PurchaseCryptocurrency;

internal sealed class PurchaseCryptocurrencyHandler : INotificationHandler<CryptocurrencyPicked>
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly ICryptocurrencyService _cryptocurrencyService;
    private readonly ILogger<PurchaseCryptocurrencyHandler> _logger;

    public PurchaseCryptocurrencyHandler(IInvestmentRepository investmentRepository,
        ICryptocurrencyService cryptocurrencyService, ILogger<PurchaseCryptocurrencyHandler> logger)
    {
        _investmentRepository = investmentRepository;
        _cryptocurrencyService = cryptocurrencyService;
        _logger = logger;
    }

    public async Task Handle(CryptocurrencyPicked notification, CancellationToken cancellationToken)
    {
        var investment = await _investmentRepository.GetAsync(notification.Id, cancellationToken);
        string? transactionId =
            await _cryptocurrencyService.BuyCryptocurrencyAsync(investment.Picked, cancellationToken);
        investment.SetTransactionId(transactionId);
        _logger.LogInformation("Set {Property} on {Object} [id={Id}, transaction_id={TransactionId}]",
            nameof(investment.TransactionId), nameof(Investment), investment.Id, investment.TransactionId);
    }
}
