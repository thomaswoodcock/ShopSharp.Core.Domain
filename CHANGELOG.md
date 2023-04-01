# Changelog

All notable changes to this project will be documented in this file.

## v0.4.0 - 2023-04-01

### Added

- `Id` property to `AggregateRoot` abstract class.

## v0.3.3 - 2023-03-30

### Changed

- Updated NUKE build to use `CombineWith` and discard variables throughout.

## v0.3.2 - 2023-03-30

### Changed

- Updated `AggregateRoot` documentation to better reflect the `UncommittedDomainEvents` type change.

## v0.3.1 - 2023-03-30

### Changed

- Changed `AggregateRoot.UncommittedDomainEvents` to `IEnumerable`.
- Moved `DomainEvent` to `ShopSharp.Core.Domain.Events` namespace.

## v0.3.0 - 2023-03-27

### Added

- `DomainEvent` abstract record.
- `AggregateRoot` abstract class.