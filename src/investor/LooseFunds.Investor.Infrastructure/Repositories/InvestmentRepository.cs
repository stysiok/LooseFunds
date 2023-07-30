using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.UnitOfWork;

namespace LooseFunds.Investor.Infrastructure.Repositories;

public sealed class InvestmentRepository : RepositoryBase, IInvestmentRepository
{
    public InvestmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public void Save(Investment investment)
        => Track(investment);

    protected override IEnumerable<Entity> ToDocument()
        => Tracked.Select(t => new InvestmentEntity { Id = t.Id });
}