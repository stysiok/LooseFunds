using LooseFunds.Investor.Core.Domain;

namespace LooseFunds.Investor.Core.Repositories;

public interface IInvestmentRepository
{
    void Save(Investment investment);
}