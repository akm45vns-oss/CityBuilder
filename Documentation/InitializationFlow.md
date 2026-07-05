# Initialization Flow

The application boots via the `Bootstrapper` coroutine in the following strict order:

1. **ConfigManager**: Loads core ScriptableObject settings (GameSettings, GraphicsSettings, etc.).
2. **ResourceManager**: Establishes asset loading pipelines (Addressables) so downstream managers can load assets.
3. **EventManager**: Prepares the event bridge (though actual events run through the static EventBus).
4. **SaveManager**: Reads disk data to prepare player profiles and preferences.
5. **InputManager**: Binds control schemes based on loaded preferences.
6. **AudioManager**: Initializes mixers and sets volumes based on saved data.
7. **SettingsManager**: Applies screen resolutions, quality levels, and VSync based on configs.
8. **PoolManager**: Pre-warms object pools for performance.
9. **GameManager**: Establishes the overarching `GameState` (transitions to Booting -> MainMenu).
10. **UIManager**: Prepares canvas structures and UI event listeners.
11. **LocalizationManager**: Loads language strings.
12. **DebugManager**: Hooks up logging UI and FPS counters (Development builds only).
13. **SceneManager (Transition)**: Automatically loads the `MainMenu` scene once everything is validated.
