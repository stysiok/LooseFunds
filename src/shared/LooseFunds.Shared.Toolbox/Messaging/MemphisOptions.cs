namespace LooseFunds.Shared.Toolbox.Messaging;

internal sealed class MemphisOptions
{
    public string? Host { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public int AccountId { get; init; }
}