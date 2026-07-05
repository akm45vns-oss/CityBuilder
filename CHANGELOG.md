# Changelog

All notable changes to the CityBuilder project will be documented in this file.

## [Milestone 01.1-Stable-Core] — 2026-07-05

### Added
- **Spatial Hash Grid**: Integrated a thread-safe O(1) spatial partitioning structure (`SpatialHashGrid<T>`) for road segment, road node, and building queries.
- **Versioned DTO Save Architecture**: Replaced direct SaveManager serialization with Data Transfer Object (DTO) structures supporting migrations (Version 1 -> Version 2) and SHA256 integrity checksum validations.
- **Command State Validation**: Added a state validation check (`Validate`) to building placement and bulldozing commands to prevent overlapping structures, invalid redos, and grid reservation corruption.
- **Testing Framework**: Added `SystemTester.cs` to run automated unit, integration, and stress tests (such as simulating 10k spatial entries under 1ms).

### Fixed
- **RoadManager/RoadSegment Compile Errors**: Resolved compile errors where `IsOccupied` was directly written to instead of using `ReservationState` and `Free()`.
- **Bootstrapper Missing Managers**: Registered `TerrainManager`, `SelectionManager`, and `PlacementManager` in the initialization routine to prevent early-boot null reference exceptions.
- **GC Allocation Spikes**: Cached renderer components on ghost previews and footprint offset lists in building definitions to eliminate per-update-frame GC allocations.
- **NaN Division Protection**: Added division-by-zero protection to `ConstructionJob.Progress` when building times are zero.

### Security
- Added checksum integrity checks for save files to detect corruption or external modifications.
