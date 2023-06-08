namespace LooseFunds.Shared.Platforms.Kraken.Models.Exceptions;

public class InvalidKrakenRequestException : Exception
{
    public InvalidKrakenRequestException(string[]? errors)
        : base($"Request returned following errors: {string.Join(", ", errors ?? Array.Empty<string>())}")
    {
    }
}