# Folder Structure

The project utilizes a highly organized standard Unity project structure:

- **Assets/**: The root of all Unity project files.
  - **Addressables/**: For dynamically loaded assets.
  - **Art/**: Contains visual assets divided into Models, Materials, Textures, and Animations.
  - **Audio/**: Contains Music, SFX, and Ambient sounds.
  - **Prefabs/**: Reusable GameObject templates.
  - **Resources/**: Legacy Unity loading (use sparingly; prefer Addressables).
  - **Scenes/**: Stores the `.unity` scene files (MainMenu, Loading, Game, etc.).
  - **ScriptableObjects/**: Data containers for configurations and item stats.
  - **Scripts/**: C# source code, further subdivided by feature:
    - **Core/**: Base classes, utilities, generic Singletons.
    - **Managers/**: Global singleton managers.
    - *(Feature folders)*: Camera, Terrain, Grid, Roads, Buildings, Economy, Population, Vehicles, Utilities, SaveSystem, UI, Audio, Editor, AI, Events, Helpers.
  - **StreamingAssets/**: For raw files needing direct path access on device.

- **Builds/**: Compiled executables (ignored by git).
- **Documentation/**: Markdown files describing the project standards and architecture.
- **Tools/**: External utilities, shell scripts, or python scripts.
- **Tests/**: Automated unit and integration tests.
- **AI/** / **Prompts/**: Storage for AI interaction memory and instructions.
