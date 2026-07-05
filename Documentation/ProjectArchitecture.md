# Project Architecture

## Overview
CityBuilder is built using Unity 6 LTS and the Universal Render Pipeline (URP). 
The architecture strongly adheres to **SOLID** principles, ensuring that components are modular, loosely coupled, and highly maintainable.

## Core Principles
1. **Component-Based Design**: Utilizing Unity's ECS or traditional MonoBehaviour component structure, favoring composition over inheritance.
2. **Singleton Managers**: Central systems (e.g., GameManager, InputManager, AudioManager) inherit from a safe generic `Singleton<T>` base class. These managers handle global states but should ideally communicate via the `EventManager` to prevent tight coupling.
3. **Dependency Injection**: While singletons are initialized, future phases will introduce DI frameworks (like VContainer or Zenject) if complexity warrants it.
4. **Data-Driven**: Heavy use of `ScriptableObjects` to define building stats, economy modifiers, and static game data rather than hardcoding values.
5. **Event-Driven Architecture**: The `EventManager` acts as a central bus to broadcast state changes (e.g., `OnPopulationChanged`, `OnTimeOfDayChanged`), preventing inter-system spaghetti code.

## Phase 0.1 Initialization
Currently, only empty base classes and structural folders are implemented. No active gameplay loop or physics simulation is running.
