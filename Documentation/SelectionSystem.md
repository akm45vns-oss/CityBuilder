# Selection System

The core gameplay loop revolves around selecting objects in the world (citizens, buildings, roads).

## Architecture
The selection system relies on three parts:
1. **`ISelectable` Interface**: Any MonoBehaviour attached to a GameObject that should be clickable must implement this interface (`GetName()`, `OnSelected()`, `OnDeselected()`).
2. **`SelectionManager`**: Listens for the `SelectAction` from the `InputManager`. When clicked, it casts a `Physics.Raycast` from the mouse pointer into the 3D world. If it hits a collider with an `ISelectable`, it selects it.
3. **`SelectionEvents`**: The `SelectionManager` broadcasts `ObjectSelectedEvent` and `ObjectDeselectedEvent` via the `EventBus`. This allows UI systems (like an info panel) to update without tightly coupling to the Selection Manager.

## Future Proofing
Currently, the system enforces **Single Selection**. The architecture is ready to be expanded into Multi-Selection by altering the `CurrentSelection` property to a `List<ISelectable>` and implementing Box-Select logic in the future.
