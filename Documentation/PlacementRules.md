# Placement Rules

The `PlacementValidator` enforces the following validation chain in order:

1. **Out of Bounds** — The footprint resolves to `null` (any cell is outside the grid).
2. **Occupied** — Any cell in the footprint has `ReservationState == Occupied`.
3. **Underwater** — Any cell's `WorldPosition.y` is below the `WaterLevel` in `GridSettings`.
4. **Slope Too Steep** — Any cell's pre-calculated slope exceeds `MaxBuildableSlope` in `GridSettings`.
5. **No Road Access** — If `RequiresRoadAccess == true`, the validator queries the `RoadNetwork` for the nearest `RoadSegment`. If none is found within a reasonable distance, placement is denied.

If all checks pass, the system returns `PlacementError.None` along with the resolved footprint cells, nearest road segment, and a projected entrance position on the road curve — directly usable by future Traffic AI pathfinding.
