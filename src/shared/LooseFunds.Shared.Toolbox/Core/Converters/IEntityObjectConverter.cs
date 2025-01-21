namespace LooseFunds.Shared.Toolbox.Core.Converters;

using Domain;
using Entity;

public interface IEntityObjectConverter<in TEntity, out TDomain>
    where TEntity : DocumentEntity where TDomain : DomainObject
{
    TDomain ToDomainObject(TEntity entity);
}
