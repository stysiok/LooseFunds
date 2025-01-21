using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Core.Converters;

public interface IEntityObjectConverter<in TEntity, out TDomain>
    where TEntity : DocumentEntity where TDomain : DomainObject
{
    TDomain ToDomainObject(TEntity entity);
}
