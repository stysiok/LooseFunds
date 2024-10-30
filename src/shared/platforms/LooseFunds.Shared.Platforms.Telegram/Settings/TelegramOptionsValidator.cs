using FluentValidation;

namespace LooseFunds.Shared.Platforms.Telegram.Settings;

internal sealed class TelegramOptionsValidator : AbstractValidator<TelegramOptions>
{
    public TelegramOptionsValidator()
    {
        RuleFor(c => c.ChatId).NotEmpty();
    }
}