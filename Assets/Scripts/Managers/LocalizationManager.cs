using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles text localization and translation switching.
    /// </summary>
    public class LocalizationManager : Singleton<LocalizationManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<LocalizationManager>(this);
            GameLogger.Info("[LocalizationManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for parsing localization CSV/JSON
    }
}
