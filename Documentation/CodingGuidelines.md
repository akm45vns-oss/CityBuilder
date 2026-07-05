# Coding Guidelines

## General Principles
- **SOLID**: Follow Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion.
- **Clean Code**: Code must be self-documenting. Use descriptive names. Do not use magic numbers; define them as `const` or `readonly` fields.
- **DRY & KISS**: Don't Repeat Yourself, Keep It Simple Stupid.
- **No TODOs**: Fix the code or log an issue. Prototype code must not be pushed to `main`.
- **Composition over Inheritance**: Prefer building functionality with multiple small MonoBehaviours rather than massive base classes.

## Naming Conventions
- **Classes / Structs / Enums**: `PascalCase` (e.g., `GameManager`, `BuildingState`).
- **Interfaces**: Start with `I` and use `PascalCase` (e.g., `IDamageable`).
- **Public Properties**: `PascalCase` (e.g., `public int CurrentPopulation { get; private set; }`).
- **Methods**: `PascalCase` (e.g., `CalculateTaxes()`).
- **Private/Protected Fields**: `_camelCase` with an underscore prefix (e.g., `private float _taxRate;`).
- **Local Variables / Parameters**: `camelCase` (e.g., `int buildingId`).
- **Enums**: Singular nouns (e.g., `enum Direction`, not `Directions`).

## Formatting
- Use `#region` to group related fields or methods if the class grows large.
- All classes must reside in the `CityBuilder` namespace or a specific sub-namespace (e.g., `CityBuilder.Managers`).
- Include XML documentation `/// <summary>` on all public classes, methods, and complex logic.
