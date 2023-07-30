namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    void AddRepository(RepositoryBase repository);
    internal Task CommitAsync(CancellationToken cancellationToken = default);
}