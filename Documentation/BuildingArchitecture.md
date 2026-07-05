# Building Architecture

CityBuilder's building system follows a strict three-layer architecture:

## Layer 1: Data (ScriptableObjects)
- **`BuildingDefinition`**: The canonical data for every building type. Defines simulation specs, footprint, economy, and asset references.
- **`BuildingDatabase`**: A centralized registry of all definitions. The UI Palette queries this for filtered lists by category or keyword search.

## Layer 2: Logic (Pure C# / Managers)
- **`PlacementValidator`**: Stateless utility. Given a grid position, definition, and rotation, it returns a `PlacementError` enum and resolves the exact cells and nearest road connection.
- **`FootprintManager`**: Stateless utility. Converts footprint offset lists (including irregular/custom shapes) into resolved `GridCell` references.
- **`PlaceBuildingCommand / BulldozeBuildingCommand`**: Command Pattern encapsulations holding pre-validated data for full undo/redo support without re-querying the world.

## Layer 3: Runtime (MonoBehaviours)
- **`Building`**: Attached to the placed GameObject. Stores live data (workers, state) and holds the road connection (`ConnectedRoadSegment`, `EntrancePosition`).
- **`PlacementManager`**: Manages the ghost preview each frame. Applies green/red tinting and routes confirmed clicks to `BuildingManager.ExecuteCommand`.
