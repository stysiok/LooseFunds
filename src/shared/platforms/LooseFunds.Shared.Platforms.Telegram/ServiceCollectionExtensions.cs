using FluentValidation;
using LooseFunds.Shared.Platforms.Telegram.Clients;
using LooseFunds.Shared.Platforms.Telegram.Clients.Fakes;
using LooseFunds.Shared.Platforms.Telegram.Settings;
using LooseFunds.Shared.Platforms.Telegram.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LooseFunds.Shared.Platforms.Telegram;

public static class ServiceCollectionExtensions
{
    public static void AddTelegram(this IServiceCollection serviceCollection, IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        var telegramConfigurationSection = configuration.GetRequiredSection(TelegramConsts.SectionName);

        VerifyTelegramOptions(telegramConfigurationSection);
        VerifyTelegramSettings(telegramConfigurationSection);

        if (hostEnvironment.IsProduction())
            serviceCollection.AddScoped<ITelegramClient, TelegramClient>();
        else
            serviceCollection.AddScoped<ITelegramClient, FakeTelegramClient>();
    }

    private static void VerifyTelegramOptions(IConfiguration configuration)
    {
        var telegramOptions = configuration.Get<TelegramOptions>();
        new TelegramOptionsValidator().ValidateAndThrow(telegramOptions);
    }

    private static void VerifyTelegramSettings(IConfiguration configuration)
    {
        var telegramOptions = configuration.Get<TelegramSettings>();
        new TelegramSettingsValidator().ValidateAndThrow(telegramOptions);
    }
}