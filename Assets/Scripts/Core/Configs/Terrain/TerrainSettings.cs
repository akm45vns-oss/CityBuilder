using UnityEngine;

namespace CityBuilder.Core.Configs.Terrain
{
    [CreateAssetMenu(fileName = "TerrainSettings", menuName = "CityBuilder/Configs/Terrain/TerrainSettings")]
    public class TerrainSettings : ScriptableObject
    {
        [Header("Generation Settings")]
        public bool UseRandomSeed = true;
        public int Seed = 0;

        [Header("Noise Parameters")]
        public float NoiseScale = 50f;
        public int Octaves = 4;
        [Range(0, 1)] public float Persistence = 0.5f;
        public float Lacunarity = 2f;
        public Vector2 Offset = Vector2.zero;

        [Header("Height Control")]
        public AnimationCurve HeightCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float BaseHeight = 0f;
        public float HeightMultiplier = 100f;
    }
}
