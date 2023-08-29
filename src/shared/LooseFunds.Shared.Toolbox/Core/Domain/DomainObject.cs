namespace LooseFunds.Shared.Toolbox.Core.Domain;

public abstract class DomainObject
{
    private readonly Queue<IDomainEvent> _domainEvents = new();

    protected DomainObject(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Enqueue(domainEvent);

    internal IDomainEvent? TryGetNextDomainEvent()
        => _domainEvents.TryDequeue(out var domainEvent) ? domainEvent : null;
}