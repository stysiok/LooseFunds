using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record AddOrder : PrivateKrakenRequest
{
    public OrderType OrderType { get; }
    public Type Type { get; }
    public decimal Volume { get; }
    public Pair Pair { get; }
    public int? Price { get; }

    public AddOrder(OrderType orderType, Type type, decimal volume, Pair pair, int? price = default) 
        : base(nameof(AddOrder))
    {
        OrderType = orderType;
        Type = type;
        Volume = volume;
        Pair = pair;
        Price = price;
        
        new AddOrderRequestValidator().ValidateAndThrow(this);
    }
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