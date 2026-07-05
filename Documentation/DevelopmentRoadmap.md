# Development Roadmap

## Phase 0.1 to 4.0 (Milestone 1) — COMPLETED
- **Foundation & Bootstrapping**: Core singleton managers, static event bus, thread-safe service locator.
- **RTS Camera System**: Movement, edge scrolling, zoom, boundaries, and raycast selection.
- **Terrain & Grid**: Multi-octave procedural Perlin heightmap, mathematical 2D grid cell slope/buildability checks, instanced foliage spawner.
- **Road Graph & Placement**: Connected graph segment/nodes with auto-junction type detection (T-junctions, cross-junctions), procedural visual meshes, and lane metadata.
- **Advanced Building System**: Irregular footprints via custom offsets, 5-state build machine, road access snapping, and undo/redo command stack history.

## Phase 4.1 — Refactoring & Stabilization (CURRENT)
- **Spatial Indexing**: Integrated O(1) `SpatialHashGrid` lookups for scaling up to 100k+ elements.
- **Save Versioning & Checksums**: Versioned DTO JSON saves with SHA256 integrity check and migration logic.
- **Thread Safety**: Concurrent service registry and lock-guarded spatial grids.
- **Zero GC Allocations**: Caching renderers on ghost previews and rectangular offsets on definitions.
- **Automated Tests**: Unit, integration, and stress tests via `SystemTester` harness.

## Phase 5.0 — Simulation Engine (FUTURE)
- **Agent-based pathfinding (A*)**.
- **Resource propagation (Water, Electricity)**.
- **Time scale and economic cycles**.
