# World Generation

CityBuilder uses a procedural approach for map generation.

## Multi-Octave Perlin Noise
The `TerrainGenerator` does not rely on a single pass of noise. It combines multiple "octaves" of noise to create realistic terrain:
- **Base Noise**: Large rolling hills (low frequency).
- **Detail Noise**: Smaller bumps and imperfections added on top (high frequency).

## Parameters
Controlled via `TerrainSettings`:
- **Scale**: The "zoom" level of the noise.
- **Octaves**: How many layers of noise are combined.
- **Persistence**: How much amplitude decreases per octave (usually ~0.5).
- **Lacunarity**: How much frequency increases per octave (usually ~2.0).
- **Seed**: A random integer determining the noise offset. Reusing a seed guarantees identical terrain generation.
