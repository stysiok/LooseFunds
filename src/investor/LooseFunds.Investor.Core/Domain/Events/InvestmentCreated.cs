using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.Events;

public sealed record InvestmentCreated(Guid Id) : IDomainEvent;