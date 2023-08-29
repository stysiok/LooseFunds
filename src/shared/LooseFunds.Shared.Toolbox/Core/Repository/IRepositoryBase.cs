using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Core.Repository;

public interface IRepositoryBase
{
    protected internal IDomainEvent? GetNextDomainEvent();
    protected internal IEnumerable<DocumentEntity> ToDocument();
}