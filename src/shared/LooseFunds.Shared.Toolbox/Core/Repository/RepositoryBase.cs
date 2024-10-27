using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Core.Repository;

public abstract class RepositoryBase<TDomain, TEntity> : IRepositoryBase
    where TDomain : DomainObject
    where TEntity : DocumentEntity
{
    private readonly IDomainObjectConverter<TDomain, TEntity> _converter;
    private readonly HashSet<DomainObject> _tracked;
    private readonly IUnitOfWork _unitOfWork;
    protected readonly ILogger Logger;
    private readonly string _domainObjectName;

    protected RepositoryBase(IDomainObjectConverter<TDomain, TEntity> converter, IUnitOfWork unitOfWork, ILogger logger)
    {
        _domainObjectName = typeof(TDomain).Name;
        _converter = converter;
        _unitOfWork = unitOfWork;
        Logger = logger;
        _tracked = new HashSet<DomainObject>();
    }

    public IDomainEvent? GetNextDomainEvent()
        => _tracked.Select(domainObject => domainObject.TryGetNextDomainEvent()).FirstOrDefault();

    public IEnumerable<DocumentEntity> ToDocument()
        => _tracked.Select(t => _converter.ToDocumentEntity((TDomain)t));

    protected void Track(DomainObject domainObject)
    {
        var wasAdded = _tracked.Add(domainObject);
        if (wasAdded is false)
        {
            Logger.LogDebug("{Object} is already being tracked [id={Id}]", _domainObjectName, domainObject.Id);
            return;
        }

        _unitOfWork.AddRepository(this);
        Logger.LogDebug("Started tracking {Object} [id={Id}]", _domainObjectName, domainObject.Id);
    }

    protected Task<TDomain> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        Logger.LogDebug("{Method} trying to get {Object} [id={Id}]", nameof(GetAsync), _domainObjectName, id);
        var domainObject = _tracked.FirstOrDefault(o => o.Id.Equals(id));

        if (domainObject is not null)
        {
            Logger.LogDebug("{Method} got {Object} [id={Id}]", nameof(GetAsync), _domainObjectName, id);
            return Task.FromResult((TDomain)domainObject);
        }

        //TODO something 😅
        Logger.LogError("{Method} didn't find any {Object} [id={Id}]", nameof(GetAsync), _domainObjectName, id);
        throw new Exception();
    }
}