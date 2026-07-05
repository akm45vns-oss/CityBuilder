using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles asynchronous scene loading, unloading, and transitions.
    /// </summary>
    public class SceneManager : Singleton<SceneManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<SceneManager>(this);
            GameLogger.Info("[SceneManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for async scene loading using Addressables
    }
}
