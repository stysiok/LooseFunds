using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.UnitOfWork;

namespace LooseFunds.Shared.Toolbox.Core.Repository;

public abstract class RepositoryBase<TDomain, TEntity> : IRepositoryBase
    where TDomain : DomainObject
    where TEntity : DocumentEntity
{
    private readonly IDomainObjectConverter<TDomain, TEntity> _converter;
    private readonly HashSet<DomainObject> _tracked;
    private readonly IUnitOfWork _unitOfWork;

    protected RepositoryBase(IDomainObjectConverter<TDomain, TEntity> converter, IUnitOfWork unitOfWork)
    {
        _converter = converter;
        _unitOfWork = unitOfWork;
        _tracked = new HashSet<DomainObject>();
    }

    public IDomainEvent? GetNextDomainEvent()
        => _tracked.Select(domainObject => domainObject.TryGetNextDomainEvent()).FirstOrDefault();

    public IEnumerable<DocumentEntity> ToDocument()
        => _tracked.Select(t => _converter.ToDocumentEntity((TDomain)t));

    protected void Track(DomainObject domainObject)
    {
        _tracked.Add(domainObject);
        _unitOfWork.AddRepository(this);
    }

    protected Task<TDomain> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var domainObject = _tracked.FirstOrDefault(o => o.Id.Equals(id));
        if (domainObject is not null) return Task.FromResult((TDomain)domainObject);

        throw new Exception();
    }
}