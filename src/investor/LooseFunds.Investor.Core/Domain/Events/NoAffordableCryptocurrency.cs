using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.Events;

public sealed record NoAffordableCryptocurrency(Guid Id) : IDomainEvent;