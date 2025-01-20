using LooseFunds.Shared.Toolbox.Core.Converters;
using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using Marten;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.Core.Repository;

public abstract class RepositoryBase<TDomain, TEntity>
    where TDomain : DomainObject
    where TEntity : DocumentEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _domainObjectName;

    protected readonly IDomainObjectConverter<TDomain, TEntity> DomainConverter;
    protected readonly IEntityObjectConverter<TEntity, TDomain> EntityConverter;
    protected readonly ILogger Logger;
    protected readonly IQuerySession QuerySession;

    protected RepositoryBase(IQuerySession querySession, IDomainObjectConverter<TDomain, TEntity> domainConverter,
        IEntityObjectConverter<TEntity, TDomain> entityConverter, IUnitOfWork unitOfWork, ILogger logger)
    {
        _domainObjectName = typeof(TDomain).Name;
        QuerySession = querySession;
        DomainConverter = domainConverter;
        EntityConverter = entityConverter;
        _unitOfWork = unitOfWork;
        Logger = logger;
    }

    protected void Track(DomainObject domainObject) =>
        _unitOfWork.Track(new Trackable<TDomain, TEntity>((TDomain)domainObject, DomainConverter));

    protected Task<TDomain> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        Logger.LogDebug("{Method} trying to get {Object} [id={Id}]", nameof(GetAsync), _domainObjectName, id);
        var domainObject = _unitOfWork.Get(id);

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