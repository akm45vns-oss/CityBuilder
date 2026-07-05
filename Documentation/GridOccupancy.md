# Grid Occupancy — Phase 4 Update

Each `GridCell` now stores rich occupancy metadata powering all gameplay systems.

## New Cell Fields (Phase 4)
| Field | Type | Purpose |
|---|---|---|
| `ReservationState` | `CellReservationState` | Free / Reserved (ghost) / Occupied |
| `OccupyingBuildingID` | `string` | GUID of the building using this cell |
| `OccupyingBuildingRotation` | `float` | Degrees of rotation at placement time |
| `HasRoadConnection` | `bool` | True if this cell is the road entrance cell |

## Irregular Footprints
Rather than bounding-box iteration, the `FootprintManager` resolves an explicit list of `Vector2Int` offsets from the anchor cell. These offsets can form any shape (L, U, ring) and are correctly rotated in 90° steps by the `RotateOffset` utility.
