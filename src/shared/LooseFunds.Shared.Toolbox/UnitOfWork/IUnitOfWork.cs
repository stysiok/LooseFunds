using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    internal DomainObject? Get(Guid id);
    internal void Track(ITrackable trackable);
    protected internal Task CommitAsync(CancellationToken cancellationToken = default);
}
