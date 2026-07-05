using UnityEngine;

namespace CityBuilder.Core.Configs.Terrain
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "CityBuilder/Configs/Terrain/GridSettings")]
    public class GridSettings : ScriptableObject
    {
        [Header("Grid Definition")]
        public float CellSize = 8f; // World units per cell
        
        [Header("Buildability")]
        public float MaxBuildableSlope = 20f;
        public float WaterLevel = 10f; // Below this is underwater and not buildable
    }
}
