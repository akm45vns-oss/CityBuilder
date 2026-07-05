using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Manages all audio playback, mixing, and volume settings (Music, SFX, Ambient).
    /// </summary>
    public class AudioManager : Singleton<AudioManager>, IService
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<AudioManager>(this);
            GameLogger.Info("[AudioManager] Initialized.");
            _isInitialized = true;
        }

        // Future implementation for AudioMixer controls and sound playback
    }
}
