using ShopSharp.Core.Domain.Aggregates;

namespace ShopSharp.Core.Domain.AggregateTests.Fakes;

internal class FakeAggregateRoot : AggregateRoot
{
    public int Value { get; private set; }

    public void AddAndApplyFakeDomainEvent(FakeDomainEvent fakeDomainEvent)
    {
        AddAndApplyDomainEvent(fakeDomainEvent);
    }

    protected override void ApplyDomainEvent(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case FakeDomainEvent fakeDomainEvent:
                Value = fakeDomainEvent.Value;
                break;

            default:
                return;
        }
    }
}
