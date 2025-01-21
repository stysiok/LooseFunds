using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;
using Type = LooseFunds.Shared.Platforms.Kraken.Models.Requests.Type;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class UserTradingService : IUserTradingService
{
    private readonly IKrakenHttpClient _client;

    public UserTradingService(IKrakenHttpClient client)
        => _client = client;

    public Task<Order> AddOrderAsync(Pair pair, decimal volume, CancellationToken cancellationToken)
        => _client.SendAsync<AddOrder, Order>(new AddOrder(OrderType.market, Type.buy, volume, pair),
            cancellationToken);
}
