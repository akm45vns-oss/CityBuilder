using UnityEngine;

namespace CityBuilder.Core.Configs.Terrain
{
    [CreateAssetMenu(fileName = "TerrainBrushSettings", menuName = "CityBuilder/Configs/Terrain/TerrainBrushSettings")]
    public class TerrainBrushSettings : ScriptableObject
    {
        [Header("Brush Controls")]
        [Range(1, 100)] public int Radius = 10;
        [Range(0.01f, 1f)] public float Strength = 0.1f;
        public AnimationCurve Falloff = AnimationCurve.EaseInOut(0, 1, 1, 0); // Center is 1, edge is 0
    }
}
