using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Core.Converters;

public interface IDomainObjectConverter<in TDomain, out TEntity>
    where TDomain : DomainObject where TEntity : DocumentEntity
{
    TEntity ToDocumentEntity(TDomain domain);
}

internal interface IDomainObjectConverter
{
    internal DocumentEntity ToDocumentEntity(DomainObject domain);
}

public interface IEntityObjectConverter<in TEntity, out TDomain>
    where TEntity : DocumentEntity where TDomain : DomainObject
{
    TDomain ToDomainObject(TEntity entity);
}