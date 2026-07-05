# Road Graph

The mathematical foundation of the city's transport system.

## Data Structures
- **`RoadNode`**: Represents a specific `Vector3` position. It can be a Dead End (1 connection), Straight/Corner (2 connections), T-Junction (3 connections), or Cross-Junction (4 connections).
- **`RoadSegment`**: The edge connecting two `RoadNodes`. It stores the assigned `RoadSettings` and a list of `SplinePoints` for curved paths.

## Intersections
Intersections are inherently defined by the `RoadNode.ConnectedSegments.Count`. 
When the `RoadMeshGenerator` renders the road, it simply checks the connection count of a node to determine if it should render a T-junction polygon, a cross-junction polygon, or nothing at all.

## Future Proofing
Because the `RoadNetwork` is purely logical, replacing the simplistic flat procedural junctions with AAA prefabs later only requires updating the `RoadMeshGenerator`—the gameplay graph remains perfectly intact.
