# Service Locator

To avoid the spaghetti code often associated with global Singletons, CityBuilder uses a central `ServiceLocator`.

## How it Works
The `ServiceLocator` is a static registry mapping Types to `IService` instances. 
When the `Bootstrapper` initializes a manager, that manager registers itself:
```csharp
ServiceLocator.Register<AudioManager>(this);
```

Other systems in the game (like a building placed by the player) request the service when needed:
```csharp
var audio = ServiceLocator.Get<AudioManager>();
audio.PlayPlacementSound();
```

## Benefits
- **Testability**: Services can be cleared (`ServiceLocator.Clear()`) or replaced with mock implementations during unit testing.
- **Clean Architecture**: It prevents components from tightly coupling to static global properties, naturally encouraging interface-driven development.
