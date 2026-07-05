# Construction System — Phase 4 Update

## Full State Machine
Buildings now transition through five explicit states:
- **Blueprint** → Initial state after `Initialize()`.
- **UnderConstruction** → `BeginConstruction()` swaps in the `ConstructionPrefab` (scaffold).
- **Completed** → `FinishConstruction()` swaps in the `MainPrefab`.
- **Disabled** → Power/water cutoff. Simulation contributions paused.
- **Abandoned** → No workers or residents. Visual degradation (future).

## Pause, Cancel, Resume
The `ConstructionManager` exposes three state controls on individual jobs:
- `PauseConstruction(building)` — Freezes the timer.
- `ResumeConstruction(building)` — Unfreezes the timer.
- `CancelConstruction(building)` — Removes from queue. Building is then available for manual control.

## Construction Progress
`ConstructionJob.Progress` returns a normalized 0→1 value for driving a UI progress bar without polling.
