using System.Collections.Immutable;
using LooseFunds.Investor.Core.Domain.ValueObjects;

namespace LooseFunds.Investor.Adapters.Kraken.Services;

public interface ICryptocurrencyService
{
    Task<IImmutableList<Cryptocurrency>> GetCryptocurrenciesAsync(CancellationToken cancellationToken);
}