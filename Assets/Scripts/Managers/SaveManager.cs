using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles saving, loading, and serializing game state data.
    /// </summary>
    public class SaveManager : Singleton<SaveManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<SaveManager>(this);
            GameLogger.Info("[SaveManager] Initialized.");
            _isInitialized = true;
        }

        public void SaveTerrainData()
        {
            GameLogger.Info("[SaveManager] Exporting Heightmap and Tree Instances (Future Implementation).");
        }

        public void SaveCityData()
        {
            GameLogger.Info("[SaveManager] Exporting RoadNetwork Graph, Buildings, and Construction Queue (Future Implementation).");
        }

        // Future implementation for JSON serialization and cloud saves
    }
}
