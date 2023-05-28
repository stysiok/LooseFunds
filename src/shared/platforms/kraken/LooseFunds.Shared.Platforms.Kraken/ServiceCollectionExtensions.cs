using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Services;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Platforms.Kraken;

public static class ServiceCollectionExtensions 
{
    public static IServiceCollection AddKraken(this IServiceCollection services, IConfiguration configuration)
    {
        var krakenConfigurationSection = configuration.GetRequiredSection(KrakenSettingsConstants.SectionName);
        
        services.Configure<KrakenCredentials>(krakenConfigurationSection)
            .Configure<KrakenOptions>(krakenConfigurationSection)
            .AddSingleton<IPrivateRequestSigner, PrivateRequestSigner>()
            .AddScoped<IMarketDataService, MarketDataService>()
            .AddHttpClient<IKrakenHttpClient, KrakenHttpClient>();

        return services;
    }
}