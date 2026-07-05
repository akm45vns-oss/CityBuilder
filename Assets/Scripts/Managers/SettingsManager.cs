using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Manages player preferences, graphic settings, and accessibility options.
    /// </summary>
    public class SettingsManager : Singleton<SettingsManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<SettingsManager>(this);
            GameLogger.Info("[SettingsManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for resolution, quality, and control bindings
    }
}
