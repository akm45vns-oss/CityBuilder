using UnityEngine;

namespace CityBuilder.Core.Configs.Terrain
{
    public enum WorldSize
    {
        Small,          // e.g. 1024x1024
        Medium,         // e.g. 2048x2048
        Large,          // e.g. 4096x4096
        ExtraLarge      // e.g. 8192x8192
    }

    [CreateAssetMenu(fileName = "WorldSettings", menuName = "CityBuilder/Configs/Terrain/WorldSettings")]
    public class WorldSettings : ScriptableObject
    {
        [Header("World Configuration")]
        public WorldSize Size = WorldSize.Medium;
        public int TerrainResolution = 2048; // Must be power of 2
        public float TerrainHeight = 600f;
    }
}
