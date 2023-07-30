using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Infrastructure.Converters;

public sealed class InvestmentDomainObjectConverter : IDomainObjectConverter<Investment, InvestmentEntity>
{
    public InvestmentEntity ToDocumentEntity(Investment domain)
        => new()
        {
            Id = domain.Id
        };
}