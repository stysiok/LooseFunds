using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Repository;
using LooseFunds.Shared.Toolbox.UnitOfWork;

namespace LooseFunds.Investor.Infrastructure.Repositories;

public sealed class InvestmentRepository : RepositoryBase<Investment, InvestmentEntity>, IInvestmentRepository
{
    public InvestmentRepository(IDomainObjectConverter<Investment, InvestmentEntity> converter, IUnitOfWork unitOfWork)
        : base(converter, unitOfWork)
    {
    }

    public void Save(Investment investment) => Track(investment);
}