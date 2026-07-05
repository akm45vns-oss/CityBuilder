using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Core.Configs.Terrain;
using CityBuilder.Grid;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Central manager for the Terrain System and Grid Foundation.
    /// </summary>
    public class TerrainManager : Singleton<TerrainManager>, IService
    {
        [Header("References")]
        public UnityEngine.Terrain ActiveTerrain;
        public UnityEngine.TerrainCollider TerrainCollider;
        
        [Header("Settings")]
        public WorldSettings WorldSettings;
        public TerrainSettings TerrainSettings;
        public GridSettings GridSettings;
        public NaturalObjectSettings NaturalSettings;

        public GridSystem Grid { get; private set; }
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;

            if (ActiveTerrain == null || WorldSettings == null || TerrainSettings == null || GridSettings == null)
            {
                GameLogger.Error("[TerrainManager] Missing references or settings.");
                return;
            }

            ServiceLocator.Register<TerrainManager>(this);

            // Apply world size
            float size = GetWorldSizeInUnits(WorldSettings.Size);
            ActiveTerrain.terrainData.size = new Vector3(size, WorldSettings.TerrainHeight, size);
            
            // 1. Generate Heightmap
            CityBuilder.Terrain.TerrainGenerator.GenerateProceduralTerrain(ActiveTerrain.terrainData, TerrainSettings);
            
            // 2. Generate Grid
            Grid = new GridSystem();
            Grid.GenerateGrid(ActiveTerrain, GridSettings);

            // 3. Spawner placeholder call
            // NaturalObjectSpawner.Spawn(ActiveTerrain, NaturalSettings);

            GameLogger.Info("[TerrainManager] Initialized.");
            _isInitialized = true;
        }

        private float GetWorldSizeInUnits(WorldSize size)
        {
            switch (size)
            {
                case WorldSize.Small: return 1024f;
                case WorldSize.Medium: return 2048f;
                case WorldSize.Large: return 4096f;
                case WorldSize.ExtraLarge: return 8192f;
                default: return 2048f;
            }
        }
    }
}
