namespace LooseFunds.Shared.Toolbox.Domain;

public abstract class DomainObject
{
    protected DomainObject(Guid id, long? version)
    {
        Id = id;
        Version = version;

        Created = DateTime.UtcNow;
    }
    
    public Guid Id { get; private init; }
    public long? Version { get; private init; }
    internal DateTime Created { get; }
    
    private readonly Queue<IDomainEvent> _domainEvents = new();
    
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Enqueue(domainEvent);
    
    internal IDomainEvent? TryGetNextDomainEvent() 
        => _domainEvents.TryDequeue(out var domainEvent) ? domainEvent : null;
}