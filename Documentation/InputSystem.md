# Input System Integration

CityBuilder exclusively uses the **Unity New Input System**.

## Implementation Detail
To avoid issues with missing `.inputactions` assets causing broken references across git clones, the `InputManager` programmatically creates the `InputActionMap` during its `Initialize()` phase. 

It defines all composite bindings (e.g., 2D Vectors for WASD, 1D Axis for Q/E) directly in C#. 

## Adding New Inputs
To add a new input action:
1. Add a public `InputAction` property to `InputManager.cs`.
2. Define the binding string inside the `CreateInputActions()` method.
3. Subscribe to the `performed` or `canceled` callbacks in the relevant system (or read the value in `Update`).
4. Ensure the system unsubscribes in its `OnDestroy` or exit methods to prevent memory leaks.
