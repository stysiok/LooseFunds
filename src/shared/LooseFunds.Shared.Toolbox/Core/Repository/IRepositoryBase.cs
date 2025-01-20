using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Core.Repository;

internal interface IRepositoryBase
{
    IDomainEvent? GetNextDomainEvent();
    IEnumerable<DocumentEntity> ToDocument();
}