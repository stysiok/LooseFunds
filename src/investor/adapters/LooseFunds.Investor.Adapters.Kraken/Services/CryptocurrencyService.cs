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

    public CryptocurrencyService(IMarketDataService marketDataService)
    {
        _marketDataService = marketDataService;
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
            var fraction = double.Parse(minOrders[t.Key].MinimumOrder);

            return new Cryptocurrency(coin, new Money(price), fraction);
        }).ToImmutableList();
    }
}