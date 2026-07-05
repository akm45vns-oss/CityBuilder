# Release Notes — Milestone 01.1-Stable-Core

We are proud to release Milestone 01.1-Stable-Core of the CityBuilder game engine. This release is dedicated to architectural stabilization, thread-safety, serialization versioning, spatial indexing scaling, and memory footprint reduction.

## Key Enhancements

1. **High-Performance Spatial Indexing**
   - Traditional O(N) searches for road networks and building footprints have been replaced with a generic, thread-safe `SpatialHashGrid<T>`. This allows the game engine to scale to **100,000+ roads and buildings** with no drop in validation frame rate, maintaining queries under 1.0 millisecond.

2. **Secure Versioned Saves**
   - The game now exports states via decoupled DTO containers, featuring automatic backward-compatible migrations (from legacy formats) and SHA256 checksums to detect file corruption.

3. **Command Validation**
   - Restored and bulldozed commands now check grid cell reservations and building lookup registry statuses before modifying scene assets. This prevents duplicates, overlaps, and grid state corruptions during rapid undo/redo cycles.

4. **Zero Allocation Preview Modes**
   - Component query loops and rectangular offset lists are now fully cached, reducing garbage collection allocation spikes in update loops to absolute zero.
