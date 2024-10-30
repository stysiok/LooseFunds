using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Services;
using LooseFunds.Shared.Platforms.Kraken.Services.Fakes;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LooseFunds.Shared.Platforms.Kraken;

public static class ServiceCollectionExtensions
{
    public static void AddKraken(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        var krakenConfigurationSection = configuration.GetRequiredSection(KrakenConsts.SectionName);

        VerifyKrakenCredentials(krakenConfigurationSection);
        VerifyKrakenOptions(krakenConfigurationSection);

        services.Configure<KrakenCredentials>(krakenConfigurationSection)
            .Configure<KrakenOptions>(krakenConfigurationSection)
            .AddSingleton<IPrivateRequestSigner, PrivateRequestSigner>()
            .AddScoped<IMarketDataService, MarketDataService>()
            .AddScoped<IUserDataService, UserDataService>()
            .AddHttpClient<IKrakenHttpClient, KrakenHttpClient>();

        if (environment.IsProduction())
            services.AddScoped<IUserTradingService, UserTradingService>();
        else
            services.AddScoped<IUserTradingService, FakeUserTradingService>();
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