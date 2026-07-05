using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Manages loading and caching of game assets, prefabs, and ScriptableObjects.
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<ResourceManager>(this);
            GameLogger.Info("[ResourceManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for Addressables loading and pooling
    }
}
