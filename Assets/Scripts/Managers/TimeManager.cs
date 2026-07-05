using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Controls the simulation speed, day/night cycle, and game time scaling.
    /// </summary>
    public class TimeManager : Singleton<TimeManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<TimeManager>(this);
            GameLogger.Info("[TimeManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for calendar tracking and time dilation
    }
}
