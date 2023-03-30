using ShopSharp.Core.Domain.Events;

namespace ShopSharp.Core.Domain.AggregateTests.Fakes;

public record FakeDomainEvent(int Value) : DomainEvent;
