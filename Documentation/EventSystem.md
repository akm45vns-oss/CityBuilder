# Event System

CityBuilder employs a highly decoupled, static `EventBus` to facilitate communication between systems without introducing hard references.

## Key Features
- **Type-Safe**: Events are defined by distinct struct or class types, making it impossible to subscribe to the wrong string literal.
- **Memory Leak Protection**: Exposes explicit `Subscribe` and `Unsubscribe` methods. Components *must* unsubscribe in their `OnDestroy` or `OnDisable` methods.
- **Global Broadcast**: Any system can call `EventBus.Broadcast(new PopulationChangedEvent { amount = 50 })`.

## Example Usage
```csharp
// Define the Event
public struct GameStateChangedEvent { public GameManager.GameState NewState; }

// Subscribe
EventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);

// Broadcast
EventBus.Broadcast(new GameStateChangedEvent { NewState = GameState.Playing });

// Unsubscribe
EventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
```
