# Road Architecture

CityBuilder implements a strict separation of concerns regarding roads: Gameplay Data vs. Visual Rendering.

## Core Managers
- **`RoadManager`**: The primary `IService` entry point. Gameplay systems interact with this to request road additions or removals.
- **`RoadNetwork`**: The pure mathematical graph containing the logic for intersections and lengths.
- **`RoadMeshGenerator`**: A Unity `MonoBehaviour` strictly responsible for generating procedural `MeshFilter` geometry based on the `RoadNetwork` data.

## Road Types
Road types are defined entirely via `RoadSettings` ScriptableObjects. This allows designers to create new roads (e.g., Highway, Dirt Road, Tram Track) simply by creating a new asset and adjusting variables like `Lanes`, `SpeedLimit`, and `BuildCostPerUnit` without touching code.
