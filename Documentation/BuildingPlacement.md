# Building Placement

The building system allows players to zone or place structures on the grid.

## `Building` Component
When a building is placed, it is assigned a runtime `Building.cs` component. This component tracks live simulation data:
- `CurrentWorkers`
- `CurrentResidents`
- `State` (Ghost, UnderConstruction, Operational, Abandoned).

## Validation Logic
The `PlacementValidator` is a pure C# utility that runs every frame while the player drags a building ghost. It checks:
1. **Bounds**: Is the building inside the grid?
2. **Occupancy**: Are the underlying `GridCells` empty?
3. **Slope**: Is the terrain too steep (`GridCell.IsBuildable`)?
4. **Water**: Is the terrain underwater?
5. **Road Access**: (Configurable) Does the building touch a road?

If validation passes, the `BuildingManager` instantiates the building and marks the underlying grid cells as occupied.
