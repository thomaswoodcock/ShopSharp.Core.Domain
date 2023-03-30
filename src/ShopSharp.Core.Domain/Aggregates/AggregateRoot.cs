using ShopSharp.Core.Domain.Events;

namespace ShopSharp.Core.Domain.Aggregates;

/// <summary>
/// Represents an aggregate root in the domain model.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _uncommittedDomainEvents = new();

    /// <summary>
    /// Gets the uncommitted domain events associated with this aggregate root.
    /// </summary>
    /// <value>
    /// An enumerable collection of uncommitted domain events.
    /// </value>
    public IEnumerable<DomainEvent> UncommittedDomainEvents => _uncommittedDomainEvents;

    /// <summary>
    /// Marks all uncommitted domain events associated with this aggregate root as committed.
    /// </summary>
    public void MarkDomainEventsAsCommitted()
    {
        _uncommittedDomainEvents.Clear();
    }

    /// <summary>
    /// Adds a domain event to the collection of uncommitted domain events and applies it to the aggregate root.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise and apply.</param>
    protected void AddAndApplyDomainEvent(DomainEvent domainEvent)
    {
        ApplyDomainEvent(domainEvent);
        _uncommittedDomainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Applies a domain event to the aggregate root. This method must be implemented in derived classes to handle the specific event.
    /// </summary>
    /// <param name="domainEvent">The domain event to apply.</param>
    protected abstract void ApplyDomainEvent(DomainEvent domainEvent);
}
