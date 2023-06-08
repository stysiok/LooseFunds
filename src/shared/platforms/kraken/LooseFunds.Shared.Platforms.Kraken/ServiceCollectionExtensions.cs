using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Services;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Platforms.Kraken;

public static class ServiceCollectionExtensions
{
    public static void AddKraken(this IServiceCollection services, IConfiguration configuration)
    {
        var krakenConfigurationSection = configuration.GetRequiredSection(KrakenSettingsConsts.SectionName);

        VerifyKrakenCredentials(krakenConfigurationSection);
        VerifyKrakenOptions(krakenConfigurationSection);

        services.Configure<KrakenCredentials>(krakenConfigurationSection)
            .Configure<KrakenOptions>(krakenConfigurationSection)
            .AddSingleton<IPrivateRequestSigner, PrivateRequestSigner>()
            .AddScoped<IMarketDataService, MarketDataService>()
            .AddScoped<IUserDataService, UserDataService>()
            .AddHttpClient<IKrakenHttpClient, KrakenHttpClient>();
    }

    private static void VerifyKrakenOptions(IConfiguration configurationSection)
    {
        var krakenOptions = configurationSection.Get<KrakenOptions>();
        new KrakenOptionsValidator().ValidateAndThrow(krakenOptions);
    }

    private static void VerifyKrakenCredentials(IConfiguration configurationSection)
    {
        var krakenCredentials = configurationSection.Get<KrakenCredentials>();
        new KrakenCredentialsValidator().ValidateAndThrow(krakenCredentials);
    }
}