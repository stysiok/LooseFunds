using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Repository;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Investor.Infrastructure.Repositories;

public sealed class InvestmentRepository : RepositoryBase<Investment, InvestmentEntity>, IInvestmentRepository
{
    public InvestmentRepository(IDomainObjectConverter<Investment, InvestmentEntity> converter, IUnitOfWork unitOfWork,
        ILogger<InvestmentRepository> logger)
        : base(null, converter, null, unitOfWork, logger)
    {
    }

    public void Save(Investment investment) => Track(investment);

    public new Task<Investment> GetAsync(Guid id, CancellationToken cancellationToken)
        => base.GetAsync(id, cancellationToken);
}