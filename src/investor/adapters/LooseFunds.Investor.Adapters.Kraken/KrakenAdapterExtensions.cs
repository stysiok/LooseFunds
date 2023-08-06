using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Shared.Platforms.Kraken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Investor.Adapters.Kraken;

public static class KrakenAdapterExtensions
{
    public static void AddKrakenAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKraken(configuration);

        services.AddScoped<IBudgetService, BudgetService>();
    }
}