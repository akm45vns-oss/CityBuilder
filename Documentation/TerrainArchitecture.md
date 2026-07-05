# Terrain Architecture

The Terrain System serves as the foundation for the entire simulation, providing the surface upon which all zoning, pathfinding, and building logic operates.

## Managers & Data
- **`TerrainManager.cs`**: The core `IService`. It orchestrates the generation of the heightmap, the generation of the grid, and the spawning of natural objects. It exposes references to the `UnityEngine.Terrain` and the generated `GridSystem`.
- **Configs**: The entire system is data-driven via `ScriptableObject` assets (`WorldSettings`, `TerrainSettings`, `GridSettings`, `NaturalObjectSettings`, etc.) preventing hardcoded magic numbers.

## Sub-Systems
1. **TerrainGenerator**: Pure C# logic that modifies the `TerrainData.SetHeights` array based on Perlin noise algorithms.
2. **TerrainEditor**: Command-pattern implementation allowing tools to deform the heightmap and modify splatmaps dynamically.
3. **NaturalObjectSpawner**: Handles placing tens of thousands of trees and rocks based on slope and elevation rules.
4. **GridSystem**: Generates a 2D array of `GridCell` objects overlapping the physical terrain, pre-calculating slopes and buildability.
