using LooseFunds.Investor.Adapters.Kraken.Services;
using LooseFunds.Shared.Platforms.Kraken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LooseFunds.Investor.Adapters.Kraken;

public static class KrakenAdapterExtensions
{
    public static void AddKrakenAdapter(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddKraken(configuration, environment);

        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<ICryptocurrencyService, CryptocurrencyService>();
    }
}