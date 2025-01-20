using LooseFunds.Investor.Core.Domain.ValueObjects;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.Events;

public sealed record InvestmentFinished(Cryptocurrency Picked, Money Budget) : IDomainEvent;