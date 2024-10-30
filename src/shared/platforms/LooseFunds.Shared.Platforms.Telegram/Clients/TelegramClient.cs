using LooseFunds.Shared.Platforms.Telegram.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace LooseFunds.Shared.Platforms.Telegram.Clients;

internal sealed class TelegramClient : ITelegramClient
{
    private readonly IOptions<TelegramOptions> _options;
    private readonly ILogger<TelegramClient> _logger;
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramClient(IOptions<TelegramSettings> settings, IOptions<TelegramOptions> options,
        ILogger<TelegramClient> logger)
    {
        _options = options;
        _logger = logger;
        _telegramBotClient = new TelegramBotClient(settings.Value.Token!);
    }

    public async Task SendMessageAsync(string content, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Sending message [message={Message}]", content);
        var message = await _telegramBotClient.SendTextMessageAsync(_options.Value.ChatId!, content,
            cancellationToken: cancellationToken);
        _logger.LogTrace("Message sent [id={Id}, type={Type}]", message.Type, message.MessageId);
    }
}