using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Investor.Infrastructure.Entities;

public sealed class InvestmentEntity : DocumentEntity
{
    public uint BudgetInPennies { get; init; }
}