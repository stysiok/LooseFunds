using LooseFunds.Shared.Platforms.Kraken.Clients;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class UserDataService : IUserDataService
{
    private readonly IKrakenHttpClient _krakenHttpClient;

    public UserDataService(IKrakenHttpClient krakenHttpClient)
    {
        _krakenHttpClient = krakenHttpClient;
    }
    
    public Task<IReadOnlyDictionary<string, decimal>> GetAccountBalanceAsync(CancellationToken cancellationToken)
        => _krakenHttpClient.PostAsync<Models.Requests.GetAccountBalance, IReadOnlyDictionary<string, decimal>>(new(),
            cancellationToken);
}