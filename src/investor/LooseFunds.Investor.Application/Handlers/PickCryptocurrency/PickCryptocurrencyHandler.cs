using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Investor.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Application.Handlers.PickCryptocurrency;

internal sealed class PickCryptocurrencyHandler : INotificationHandler<CryptocurrenciesSet>
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly ILogger<PickCryptocurrencyHandler> _logger;

    public PickCryptocurrencyHandler(IInvestmentRepository investmentRepository,
        ILogger<PickCryptocurrencyHandler> logger)
    {
        _investmentRepository = investmentRepository;
        _logger = logger;
    }

    public async Task Handle(CryptocurrenciesSet notification, CancellationToken cancellationToken)
    {
        var investment = await _investmentRepository.GetAsync(notification.Id, cancellationToken);
        investment.PickCryptocurrency();
        _logger.LogInformation("Picked {Property} on {Object} [id={Id}, picked_coin={PickedCoin}]",
            nameof(Cryptocurrency), nameof(Investment), investment.Id, investment.Picked.Coin);
    }
}