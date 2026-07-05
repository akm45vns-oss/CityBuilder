# Grid System

The physical Unity `Terrain` mesh is difficult to query rapidly for gameplay logic. Therefore, we overlay a mathematical `GridSystem` across the terrain.

## GridCell Data
The terrain is divided into `GridCell[,]`. Each cell stores:
- X, Z coordinates.
- World Position (X, Y, Z).
- Pre-calculated **Slope** (Steepness).
- **IsBuildable** (False if slope > `MaxBuildableSlope` or if Y < `WaterLevel`).

## Usage
When the player attempts to place a road or zone a building, the placement system queries `TerrainManager.Instance.Grid.GetCellFromWorldPosition()`. The game instantly knows if the area is flat enough to build on without needing expensive physics raycasts against the terrain mesh.
