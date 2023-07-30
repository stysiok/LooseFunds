using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.Events;

public record InvestmentCreated(Guid Id) : IDomainEvent;