using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests;

internal sealed record GetAccountBalance() : PrivateKrakenRequest(nameof(GetAccountBalance));