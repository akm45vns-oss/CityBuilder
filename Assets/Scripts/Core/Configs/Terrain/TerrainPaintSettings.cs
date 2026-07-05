using UnityEngine;
using System.Collections.Generic;

namespace CityBuilder.Core.Configs.Terrain
{
    [System.Serializable]
    public struct TerrainLayerInfo
    {
        public string Name;
        public Texture2D DiffuseTexture;
        public Texture2D NormalMap;
        public Vector2 TileSize;
    }

    [CreateAssetMenu(fileName = "TerrainPaintSettings", menuName = "CityBuilder/Configs/Terrain/TerrainPaintSettings")]
    public class TerrainPaintSettings : ScriptableObject
    {
        [Header("Layers")]
        public List<TerrainLayerInfo> Layers;

        [Header("Auto Texturing Rules")]
        [Tooltip("Layer index for base ground")]
        public int BaseLayerIndex = 0;
        [Tooltip("Layer index for steep cliffs")]
        public int CliffLayerIndex = 1;
        public float CliffMinSlope = 30f;
    }
}
