using LooseFunds.Shared.Toolbox.Domain;

namespace LooseFunds.Investor.Core.Domain.Events;

public record InvestmentCreated(Guid Id) : IDomainEvent;