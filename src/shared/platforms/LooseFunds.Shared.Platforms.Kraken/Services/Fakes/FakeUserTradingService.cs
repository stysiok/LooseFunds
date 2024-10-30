using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Platforms.Kraken.Services.Fakes;

internal sealed class FakeUserTradingService : IUserTradingService
{
    private readonly ILogger<FakeUserTradingService> _logger;

    public FakeUserTradingService(ILogger<FakeUserTradingService> logger)
    {
        _logger = logger;
    }

    public Task<Order> AddOrderAsync(Pair pair, decimal volume, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Using {Service} to generate fake {Object} [pair={Pair}, volume={Volume}]",
            nameof(FakeUserTradingService), nameof(Order), pair, volume);
        var fakeOrderInformation = new OrderInformation($"Bought {pair}@{volume}", "");
        var fakeOrder = new Order(fakeOrderInformation, new[] { Guid.NewGuid().ToString("N") });

        _logger.LogDebug("Returning fake {Object} [description={Description}, transactionId={TransactionId}]",
            nameof(Order), fakeOrder.OrderInformation.Description, fakeOrder.TransactionsIds[0]);

        return Task.FromResult(fakeOrder);
    }
}