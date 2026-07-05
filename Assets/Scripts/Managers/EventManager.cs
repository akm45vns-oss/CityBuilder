using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Acts as an optional bridge or wrapper for the static EventBus, 
    /// or for handling lifecycle events specific to the manager system.
    /// </summary>
    public class EventManager : Singleton<EventManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<EventManager>(this);
            GameLogger.Info("[EventManager] Initialized.");
            _isInitialized = true;
        }

        // The actual event routing is primarily handled by the static CityBuilder.Events.EventBus
    }
}
