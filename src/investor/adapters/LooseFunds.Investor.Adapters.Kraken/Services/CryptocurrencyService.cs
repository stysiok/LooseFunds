using System.Collections.Immutable;
using LooseFunds.Investor.Adapters.Kraken.Mappers;
using LooseFunds.Investor.Core.Domain.Consts;
using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Services;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

internal sealed class CryptocurrencyService : ICryptocurrencyService
{
    private readonly IMarketDataService _marketDataService;
    private readonly IUserTradingService _userTradingService;

    public CryptocurrencyService(IMarketDataService marketDataService, IUserTradingService userTradingService)
    {
        _marketDataService = marketDataService;
        _userTradingService = userTradingService;
    }

    public async Task<IImmutableList<Cryptocurrency>> GetCryptocurrenciesAsync(CancellationToken cancellationToken)
    {
        var pairs = Enum.GetValues<Coin>().Select(CoinPairMapper.ToPair).ToList();
        var tickers = await _marketDataService.GetTickerInfoAsync(pairs, cancellationToken);
        var minOrders = await _marketDataService.GetTradableAssetPairAsync(pairs, cancellationToken);

        return tickers.Select(t =>
        {
            var coin = CoinPairMapper.ToCoin(Enum.Parse<Pair>(t.Key));
            var price = decimal.Parse(t.Value.LastTradeClosed[0]);
            var fraction = decimal.Parse(minOrders[t.Key].MinimumOrder);

            return new Cryptocurrency(coin, new Money(price), fraction);
        }).ToImmutableList();
    }

    public async Task<string> BuyCryptocurrencyAsync(Cryptocurrency cryptocurrency, CancellationToken cancellationToken)
    {
        var pair = CoinPairMapper.ToPair(cryptocurrency.Coin);
        var order = await _userTradingService.AddOrderAsync(pair, cryptocurrency.MinimalFraction, cancellationToken);

        return order.TransactionsIds.Single();
    }
}