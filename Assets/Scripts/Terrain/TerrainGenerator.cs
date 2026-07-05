using UnityEngine;

namespace CityBuilder.Terrain
{
    using Core.Configs.Terrain;
    using Core.Logging;

    public class TerrainGenerator
    {
        public static TerrainData GenerateProceduralTerrain(TerrainData terrainData, TerrainSettings settings)
        {
            int width = terrainData.heightmapResolution;
            int length = terrainData.heightmapResolution;
            
            float[,] heights = new float[width, length];

            int seed = settings.UseRandomSeed ? Random.Range(0, 999999) : settings.Seed;
            System.Random prng = new System.Random(seed);
            
            Vector2[] octaveOffsets = new Vector2[settings.Octaves];
            for (int i = 0; i < settings.Octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + settings.Offset.x;
                float offsetY = prng.Next(-100000, 100000) + settings.Offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < settings.Octaves; i++)
            {
                maxPossibleHeight += amplitude;
                amplitude *= settings.Persistence;
            }

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    amplitude = 1;
                    frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < settings.Octaves; i++)
                    {
                        float sampleX = x / settings.NoiseScale * frequency + octaveOffsets[i].x;
                        float sampleY = y / settings.NoiseScale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= settings.Persistence;
                        frequency *= settings.Lacunarity;
                    }

                    // Normalize height
                    float normalizedHeight = (noiseHeight + 1) / (2f * maxPossibleHeight / 1.75f);
                    normalizedHeight = Mathf.Clamp01(normalizedHeight);
                    
                    // Apply curve and base height
                    float finalHeight = settings.BaseHeight + (settings.HeightCurve.Evaluate(normalizedHeight) * settings.HeightMultiplier);
                    
                    // Unity heights are expected between 0 and 1, where 1 is the terrain's max height
                    heights[y, x] = Mathf.Clamp01(finalHeight / terrainData.size.y);
                }
            }

            terrainData.SetHeights(0, 0, heights);
            GameLogger.Info($"[TerrainGenerator] Procedural heightmap generated with seed {seed}.");
            return terrainData;
        }
    }
}
