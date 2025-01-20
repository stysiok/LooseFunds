using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public interface IUnitOfWork
{
    internal DomainObject? Get(Guid id);
    internal void Track(ITrackable trackable);
    protected internal Task CommitAsync(CancellationToken cancellationToken = default);
}

internal sealed record Trackable<TDomain, TEntity>(
    TDomain DomainObject,
    IDomainObjectConverter<TDomain, TEntity> Converter)
    : ITrackable where TDomain : DomainObject where TEntity : DocumentEntity
{
    public DomainObject Tracked => DomainObject;
    public DocumentEntity Convert() => Converter.ToDocumentEntity(DomainObject);
}

internal interface ITrackable
{
    DomainObject Tracked { get; }
    DocumentEntity Convert();
}