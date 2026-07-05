using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Loads and provides access to ScriptableObject configurations.
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<ConfigManager>(this);
            GameLogger.Info("[ConfigManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for caching Config SOs
    }
}
