using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal sealed record Trackable<TDomain, TEntity>(
    TDomain DomainObject,
    IDomainObjectConverter<TDomain, TEntity> Converter)
    : ITrackable where TDomain : DomainObject where TEntity : DocumentEntity
{
    public DomainObject Instance => DomainObject;
    public DocumentEntity Convert() => Converter.ToDocumentEntity(DomainObject);
}
