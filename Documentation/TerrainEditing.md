# Terrain Editing

The `TerrainEditor` class provides the API for in-game terraforming tools.

## Supported Operations
- **Raise / Lower**: Modifies heights incrementally using a brush falloff curve.
- **Flatten / Level**: Lerps heights within a radius towards a specific target height.
- **Smooth**: Averages neighboring heights to remove jagged cliffs.

## Undo / Redo (Command Pattern)
Every editing action is wrapped in an `ITerrainCommand` (e.g., `HeightmapModifyCommand`). Before an edit occurs, the affected area's `float[,] oldHeights` is stored in the command. The command is then pushed to the `_undoStack`. Calling `Undo()` applies the old heights, and pushes the command to the `_redoStack`.

*Note: In a production environment, commands should be batched on Mouse Up to prevent storing thousands of commands per second of dragging.*
