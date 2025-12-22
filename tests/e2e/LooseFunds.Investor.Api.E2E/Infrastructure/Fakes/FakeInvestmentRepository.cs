using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Repositories;

namespace LooseFunds.Investor.Api.E2E.Infrastructure.Fakes;

internal sealed class FakeInvestmentRepository : IInvestmentRepository
{
    public void Save(Investment investment)
    {
    }

    public Task<Investment> GetAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(Investment.Create());
}
