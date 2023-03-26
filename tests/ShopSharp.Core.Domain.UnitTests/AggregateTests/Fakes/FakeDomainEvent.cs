using ShopSharp.Core.Domain.Aggregates;

namespace ShopSharp.Core.Domain.AggregateTests.Fakes;

public record FakeDomainEvent(int Value) : DomainEvent;
