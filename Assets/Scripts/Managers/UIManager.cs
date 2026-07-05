using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles all high-level UI routing, HUD interactions, and popup dialogues.
    /// </summary>
    public class UIManager : Singleton<UIManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<UIManager>(this);
            GameLogger.Info("[UIManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for UI view switching and HUD updates
    }
}
