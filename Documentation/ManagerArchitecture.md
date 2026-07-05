# Manager Architecture

CityBuilder utilizes a hybrid Singleton-ServiceLocator architecture for its core systems.

## Design Rules
1. **Singleton Fallback**: Managers inherit from a safe generic `Singleton<T>` to prevent duplicate GameObjects and ensure they persist via `DontDestroyOnLoad`.
2. **Interface Implementation**: Every manager implements `IService`, which mandates a public `Initialize()` method.
3. **Registration**: During `Initialize()`, each manager registers itself with the `ServiceLocator`.
4. **Decoupling**: While managers technically have an `.Instance` property due to the Singleton base, other systems **must** request them via `ServiceLocator.Get<T>()` to enforce dependency inversion and allow for easier mocking/testing in the future.
5. **No Circular Dependencies**: Managers must not rely on each other's `Awake` methods. All inter-manager setup should happen post-registration or via the global EventBus.
