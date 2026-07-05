using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Central manager for the game state, orchestrating high-level logic and phase transitions.
    /// </summary>
    public class GameManager : Singleton<GameManager>, IService
    {
        public enum GameState
        {
            Booting,
            Loading,
            MainMenu,
            Playing,
            Paused,
            Saving,
            LoadingSave,
            Exiting
        }

        private GameState _currentState;
        private bool _isInitialized;

        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                // Future: CityBuilder.Events.EventBus.Broadcast(new GameStateChangedEvent(value));
            }
        }

        public void Initialize()
        {
            if (_isInitialized) return;

            ServiceLocator.Register<GameManager>(this);
            CurrentState = GameState.Booting;
            GameLogger.Info("[GameManager] Initialized.");
            
            _isInitialized = true;
        }

        /// <summary>
        /// Transitions the game to the specified state.
        /// </summary>
        public void SetGameState(GameState newState)
        {
            CurrentState = newState;
            GameLogger.Info($"[GameManager] Game State changed to {newState}");
        }
    }
}
