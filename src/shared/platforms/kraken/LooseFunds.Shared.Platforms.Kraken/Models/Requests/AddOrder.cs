using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record AddOrder
    (OrderType OrderType, Type Type, decimal Volume, string Pair, int Price) : PrivateKrakenRequest
{
    public override string Pathname => $"{base.Pathname}AddOrder";
}

internal enum OrderType
{
    market,
    limit
}

internal enum Type
{
    buy
}