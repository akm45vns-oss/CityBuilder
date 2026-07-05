# Migration Notes — Milestone 01.1-Stable-Core

This document details the transition from raw serialization structures in Milestone 01.0 to the new versioned DTO and migration pipelines in Milestone 01.1.

## Upgraded Save Formats

- **Version 1 (Legacy)**: Serialized raw manager properties directly to file arrays. Lacked version headers and checksum validation.
- **Version 2 (Current)**: Exports states via unified `SaveDataDTO.cs` wrappers with an explicit `SaveVersion` header and a unique `Checksum` string.

## Migration Pipeline

The `SaveMigrationSystem` automatically upgrades older JSON formats on load:
1. Detects `SaveVersion` header. If missing, it defaults to Version 1.
2. Applies sequential migration handlers (e.g. `MigrateV1ToV2`) to map deprecated arrays to current DTO formats.
3. Overwrites the file using the upgraded schema and a fresh SHA256 checksum on the next save cycle.

## Code Upgrades

Developers must note the following interface migrations:
- **Footprints**: Call `FootprintManager.ResolveCells()` instead of direct coordinate math.
- **Service Locator**: All lookups are now safe to execute concurrently from background threads.
- **Grid Cell States**: Use `cell.ReservationState` instead of writing to `IsOccupied` directly.
- **Spatial Queries**: Use `RoadManager.Instance.SegmentSpatialGrid` rather than iterating `RoadManager.Instance.Network.Segments.Values`.
