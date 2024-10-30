namespace LooseFunds.Shared.Platforms.Telegram.Clients;

public interface ITelegramClient
{
    Task SendMessageAsync(string content, CancellationToken cancellationToken);
}