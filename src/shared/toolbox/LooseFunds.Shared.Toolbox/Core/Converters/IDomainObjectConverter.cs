using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Core.Converters;

public interface IDomainObjectConverter<in TDomain, out TEntity>
    where TDomain : DomainObject where TEntity : DocumentEntity
{
    TEntity ToDocumentEntity(TDomain domain);
}