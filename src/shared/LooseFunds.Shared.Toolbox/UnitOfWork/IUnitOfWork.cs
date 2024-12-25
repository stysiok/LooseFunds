using LooseFunds.Shared.Toolbox.Core.Repository;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    internal void AddRepository(IRepositoryBase repository);
    protected internal Task CommitAsync(CancellationToken cancellationToken = default);
}