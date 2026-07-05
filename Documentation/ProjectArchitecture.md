# Project Architecture

## Overview
CityBuilder is built using Unity 6 LTS and the Universal Render Pipeline (URP). 
The architecture strongly adheres to **SOLID** principles, ensuring that components are modular, loosely coupled, and highly maintainable.

## Core Principles
1. **Component-Based Design**: Reusable GameObjects with decoupled MonoBehaviours (e.g. `Building`, `RoadMeshGenerator`), favoring composition over inheritance.
2. **Service Locator**: All managers register themselves to a thread-safe `ServiceLocator` via `IService` interface to completely decouple dependencies.
3. **Thread-Safety**: Service locator registry, spatial hash grids, and locks are fully concurrent-safe, preparing for future Unity C# Job System and Burst optimizations.
4. **Data-Driven Configuration**: Heavy use of `ScriptableObjects` (`BuildingDefinition`, `BuildingDatabase`, `RoadSettings`, `GridSettings`) rather than hardcoding values.
5. **Event-Driven Architecture**: Decoupled event bus (`EventBus.cs`) for broadcasting state changes (e.g. `ObjectSelectedEvent`, `ObjectDeselectedEvent`).
6. **Spatial Indexing**: O(1) `SpatialHashGrid<T>` indexes segments, nodes, and buildings to scale up to 100,000+ items without performance degradation.
7. **Versioned DTO Save System**: Complete serialization separation via Data Transfer Objects (DTO) with SHA256 integrity checks and automatic migration pipelines.
