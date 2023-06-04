using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;

namespace LooseFunds.Shared.Platforms.Kraken.Services;

internal sealed class UserDataService : IUserDataService
{
    private readonly IKrakenHttpClient _krakenHttpClient;

    public UserDataService(IKrakenHttpClient krakenHttpClient)
    {
        _krakenHttpClient = krakenHttpClient;
    }
    
    public Task<GetAccountBalance> GetAccountBalanceAsync(CancellationToken cancellationToken)
        => _krakenHttpClient.PostAsync<Models.Requests.GetAccountBalance, GetAccountBalance>(new(), cancellationToken);
}