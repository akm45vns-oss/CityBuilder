# Building Database

The `BuildingDatabase` ScriptableObject is the game's catalog of every building type.

## Usage
The database is initialized during the Bootstrapper sequence (a `ConfigManager` method call). Once initialized, all lookups run via a `Dictionary<string, BuildingDefinition>` for O(1) access.

## Adding a New Building
1. Create a `BuildingDefinition` asset: Right-click → Create → CityBuilder → Buildings → BuildingDefinition.
2. Fill in the `BuildingID` (must be unique), name, category, and all gameplay specs.
3. Drag the asset into the `BuildingDatabase` SO's `_definitions` list.
4. The building is automatically available to the UI Palette and Placement System at runtime.

## Categories
Categories are defined in the `BuildingCategory` enum and can be filtered from the database with `GetByCategory()`. Future DLC support can add new categories here without breaking existing save data.
