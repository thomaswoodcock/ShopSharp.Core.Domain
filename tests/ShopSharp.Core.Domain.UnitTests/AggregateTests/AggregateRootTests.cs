using ShopSharp.Core.Domain.AggregateTests.Fakes;

namespace ShopSharp.Core.Domain.AggregateTests;

public class AggregateRootTests
{
    private readonly FakeAggregateRoot _aggregateRoot = new();

    [Theory]
    [AutoData]
    public void AddAndApplyDomainEventAppliesDomainEvent(FakeDomainEvent domainEvent)
    {
        // Arrange Act
        _aggregateRoot.AddAndApplyFakeDomainEvent(domainEvent);

        // Assert
        _aggregateRoot.Value
            .Should()
            .Be(domainEvent.Value);
    }

    [Theory]
    [AutoData]
    public void AddAndApplyDomainEventAddsDomainEventToUncommittedDomainEvents(FakeDomainEvent domainEvent)
    {
        // Arrange Act
        _aggregateRoot.AddAndApplyFakeDomainEvent(domainEvent);

        // Assert
        _aggregateRoot.UncommittedDomainEvents
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(domainEvent);
    }

    [Theory]
    [AutoData]
    public void MarkDomainEventsAsCommittedClearsUncommittedDomainEvents(FakeDomainEvent domainEvent)
    {
        // Arrange
        _aggregateRoot.AddAndApplyFakeDomainEvent(domainEvent);

        // Act
        _aggregateRoot.MarkDomainEventsAsCommitted();

        // Assert
        _aggregateRoot.UncommittedDomainEvents
            .Should()
            .BeEmpty();
    }
}
