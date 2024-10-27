using System.Collections.Immutable;
using LooseFunds.Investor.Adapters.Kraken.Mappers;
using LooseFunds.Investor.Core.Domain.Consts;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Services;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

internal sealed class CryptocurrencyService : ICryptocurrencyService
{
    private readonly IMarketDataService _marketDataService;
    private readonly IUserTradingService _userTradingService;
    private readonly ILogger<CryptocurrencyService> _logger;

    public CryptocurrencyService(IMarketDataService marketDataService, IUserTradingService userTradingService,
        ILogger<CryptocurrencyService> logger)
    {
        _marketDataService = marketDataService;
        _userTradingService = userTradingService;
        _logger = logger;
    }

    public async Task<IImmutableList<Cryptocurrency>> GetCryptocurrenciesAsync(CancellationToken cancellationToken)
    {
        var pairs = Enum.GetValues<Coin>().Select(CoinPairMapper.ToPair).ToList();
        _logger.LogDebug("Mapped all available coins to pairs");

        var tickers = _marketDataService.GetTickerInfoAsync(pairs, cancellationToken);

        var minOrders = _marketDataService.GetTradableAssetPairAsync(pairs, cancellationToken);

        await Task.WhenAll(tickers, minOrders);

        _logger.LogDebug("Got all min orders and tickers for pairs");

        return tickers.Result.Select(t =>
        {
            var coin = CoinPairMapper.ToCoin(Enum.Parse<Pair>(t.Key));
            var price = decimal.Parse(t.Value.LastTradeClosed[0]);
            var fraction = decimal.Parse(minOrders.Result[t.Key].MinimumOrder);

            _logger.LogDebug("Converting to {Object} [coin={Coin}, price={Price}, fraction={Fraction}]",
                nameof(Cryptocurrency), coin, price, fraction);
            return new Cryptocurrency(coin, new Money(price), fraction);
        }).ToImmutableList();
    }

    public async Task<string?> BuyCryptocurrencyAsync(Cryptocurrency? cryptocurrency,
        CancellationToken cancellationToken)
    {
        var pair = CoinPairMapper.ToPair(cryptocurrency.Coin);
        _logger.LogDebug("Mapped {Object} to {Object} [coin={Coin}, pair={Pair}]", nameof(Coin), nameof(Pair),
            cryptocurrency.Coin, pair);
        
        var order = await _userTradingService.AddOrderAsync(pair, cryptocurrency.MinimalFraction, cancellationToken);
        var transactionId = order.TransactionsIds.Single();
        _logger.LogDebug("Ordered {Object} [pair={Pair}, transaction_id={TransactionId}]", nameof(Pair), pair,
            transactionId);

        return transactionId;
    }
}