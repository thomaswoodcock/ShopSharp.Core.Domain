# ShopSharp.Core.Domain

## Table of Contents

1. [Introduction](#introduction)
2. [Installation](#installation)
3. [Usage](#usage)
   - [DomainEvent](#domainevent)
   - [AggregateRoot](#aggregateroot)
4. [Examples](#examples)
    - [Creating a Custom Domain Event](#creating-a-custom-domain-event)
    - [Creating an Aggregate Root](#creating-an-aggregate-root)

## Introduction

`ShopSharp.Core.Domain` is a NuGet package that provides core components of the ShopSharp domain layer.

## Installation

To install the `ShopSharp.Core.Domain` NuGet package, run the following command in your Package Manager Console:

```
Install-Package ShopSharp.Core.Domain
```

Alternatively, you can search for `ShopSharp.Core.Domain` in the NuGet Package Manager UI and install from there.

## Usage

### DomainEvent

`DomainEvent` is an abstract record representing an event that occurs in the domain model.
It serves as the base record for custom domain events.

### AggregateRoot

`AggregateRoot` is an abstract class representing an aggregate root in the domain model.
It is responsible for managing and applying domain events to the aggregate root.
It also keeps track of uncommitted domain events and provides a method to mark them as committed.

## Examples

### Creating a Custom Domain Event

To create a custom domain event, derive a new record from the `DomainEvent` abstract record:

```csharp
using ShopSharp.Core.Domain.Events;

public record OrderPlaced(Guid OrderId, DateTime OrderDate) : DomainEvent;
```

### Creating an Aggregate Root

To create an aggregate root, derive a new class from the `AggregateRoot` abstract class and implement the `ApplyDomainEvent` method:

```csharp
using ShopSharp.Core.Domain.Aggregates;
using ShopSharp.Core.Domain.Events;

public class Order : AggregateRoot
{
    public DateTime OrderDate { get; private set; }

    public void PlaceOrder(Guid id, DateTime orderDate)
    {
        AddAndApplyDomainEvent(new OrderPlaced(id, orderDate));
    }

    protected override void ApplyDomainEvent(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case OrderPlaced orderPlaced:
                Id = orderPlaced.OrderId;
                OrderDate = orderPlaced.OrderDate;
                break;

            default:
                throw new NotImplementedException();
        }
    }
}
```