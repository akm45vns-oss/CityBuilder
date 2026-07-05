using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Manages object pooling for performance (e.g. citizens, vehicles, particles).
    /// </summary>
    public class PoolManager : Singleton<PoolManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<PoolManager>(this);
            GameLogger.Info("[PoolManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for generic object pooling
    }
}
