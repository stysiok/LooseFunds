using LooseFunds.Investor.Core.Domain.Consts;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;

namespace LooseFunds.Investor.Adapters.Kraken.Mappers;

public static class CoinPairMapper
{
    public static Pair ToPair(Coin coin)
        => coin switch
        {
            Coin.Bitcoin => Pair.XXBTZEUR,
            Coin.Etherium => Pair.XETHZEUR,
            Coin.Ripple => Pair.XXRPZEUR,
            Coin.Cardano => Pair.ADAEUR,
            Coin.Solana => Pair.SOLEUR,
            Coin.ShibaInu => Pair.SHIBEUR,
            _ => throw new ArgumentOutOfRangeException(nameof(coin), coin, null)
        };

    public static Coin ToCoin(Pair pair)
        => pair switch
        {
            Pair.XXBTZEUR => Coin.Bitcoin,
            Pair.XETHZEUR => Coin.Etherium,
            Pair.XXRPZEUR => Coin.Ripple,
            Pair.ADAEUR => Coin.Cardano,
            Pair.SOLEUR => Coin.Solana,
            Pair.SHIBEUR => Coin.ShibaInu,
            _ => throw new ArgumentOutOfRangeException(nameof(pair), pair, null)
        };
}