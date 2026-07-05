# Terrain Optimization

Modern city builders require immense scale. Spawning 50,000 standard Unity Prefabs (GameObjects) for trees will cripple the CPU due to Transform hierarchy updates and MonoBehavior overhead.

## Natural Object Optimization
The `NaturalObjectSpawner` exclusively uses Unity's **Terrain Tree Instances**. 
- These are purely data structs containing position, rotation, and an index pointing to a `TreePrototype`. 
- Unity renders these directly on the GPU using **GPU Instancing**.
- They feature automatic LOD (Level of Detail) and billboard swapping at a distance.

## Grid Optimization
The `GridSystem` is generated once during `Bootstrapper` initialization. The data (`IsBuildable`, `Slope`) is cached in memory. Future pathfinding (A*) will iterate over this lightweight grid instead of querying physics colliders, ensuring pathfinding remains fast even with thousands of agents.
