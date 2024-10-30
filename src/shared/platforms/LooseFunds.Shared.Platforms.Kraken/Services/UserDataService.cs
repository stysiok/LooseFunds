using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class UserDataService : IUserDataService
{
    private readonly IKrakenHttpClient _client;

    public UserDataService(IKrakenHttpClient client)
        => _client = client;

    public Task<IReadOnlyDictionary<string, decimal>> GetAccountBalanceAsync(CancellationToken cancellationToken)
        => _client.SendAsync<GetAccountBalance, IReadOnlyDictionary<string, decimal>>(new(),
            cancellationToken);
}