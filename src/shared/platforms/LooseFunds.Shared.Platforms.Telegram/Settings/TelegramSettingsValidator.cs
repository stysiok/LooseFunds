using FluentValidation;

namespace LooseFunds.Shared.Platforms.Telegram.Settings;

internal sealed class TelegramSettingsValidator : AbstractValidator<TelegramSettings>
{
    public TelegramSettingsValidator()
    {
        RuleFor(c => c.Token).NotEmpty();
    }
}