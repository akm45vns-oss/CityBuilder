# Bootstrap Architecture

The Bootstrap system is the single entry point for CityBuilder's application lifecycle, ensuring a predictable, crash-free startup sequence.

## Core Component: Bootstrapper.cs
- Resides exclusively in the `Bootstrap.unity` scene.
- Automatically prevents duplicate execution via a static `_hasBootstrapped` flag.
- Invokes a strict Coroutine sequence that guarantees dependencies are loaded in the correct order before other systems access them.
- Features robust exception handling: if any manager fails to initialize, the `GameLogger` catches it and halts the sequence, preventing silent or cascaded crashes later in gameplay.
