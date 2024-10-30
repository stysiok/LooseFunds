using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Platforms.Telegram.Clients.Fakes;

internal sealed class FakeTelegramClient : ITelegramClient
{
    private readonly ILogger<FakeTelegramClient> _logger;

    public FakeTelegramClient(ILogger<FakeTelegramClient> logger)
    {
        _logger = logger;
    }

    public Task SendMessageAsync(string content, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fake sending message [message={Message}]", content);
        return Task.CompletedTask;
    }
}