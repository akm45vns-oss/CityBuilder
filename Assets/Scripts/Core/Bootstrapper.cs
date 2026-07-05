using UnityEngine;
using System.Collections;
using CityBuilder.Core.Logging;
using CityBuilder.Managers;

namespace CityBuilder.Core
{
    /// <summary>
    /// Orchestrates the strict initialization sequence of the application.
    /// Lives only in the Bootstrap scene.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private static bool _hasBootstrapped = false;

        private void Start()
        {
            if (_hasBootstrapped)
            {
                GameLogger.Warning("Bootstrapper already executed. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            _hasBootstrapped = true;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            GameLogger.Info("--- BOOTSTRAP SEQUENCE START ---");

            try
            {
                // Order: Config -> Resource -> Event (No object yet) -> Save -> Input -> Audio -> Settings -> Pool -> Game -> UI
                
                InitializeManager(ConfigManager.Instance);
                yield return null;

                InitializeManager(ResourceManager.Instance);
                yield return null;

                // EventManager doesn't exist as a GameObject in the new structure (it's a static EventBus), 
                // but if we keep the EventManager wrapper for consistency:
                InitializeManager(EventManager.Instance);
                yield return null;

                InitializeManager(SaveManager.Instance);
                yield return null;

                InitializeManager(InputManager.Instance);
                yield return null;

                InitializeManager(AudioManager.Instance);
                yield return null;

                InitializeManager(SettingsManager.Instance);
                yield return null;

                InitializeManager(PoolManager.Instance);
                yield return null;

                InitializeManager(GameManager.Instance);
                yield return null;

                InitializeManager(TerrainManager.Instance);
                yield return null;

                InitializeManager(SelectionManager.Instance);
                yield return null;

                InitializeManager(RoadManager.Instance);
                yield return null;
                
                InitializeManager(BuildingManager.Instance);
                yield return null;
                
                InitializeManager(ConstructionManager.Instance);
                yield return null;

                InitializeManager(PlacementManager.Instance);
                yield return null;
                
                InitializeManager(ToolManager.Instance);
                yield return null;

                InitializeManager(UIManager.Instance);
                yield return null;

                InitializeManager(LocalizationManager.Instance);
                yield return null;

                InitializeManager(DebugManager.Instance);
                yield return null;

            }
            catch (System.Exception e)
            {
                GameLogger.Exception(e);
                GameLogger.Error("CRITICAL FAILURE DURING BOOTSTRAP. Halting.");
                yield break;
            }

            GameLogger.Info("--- BOOTSTRAP SEQUENCE COMPLETE ---");

            // Load Main Menu using Unity's underlying SceneManager wrapper via our SceneManager
            // Assuming SceneManager.Instance is ready (we should init it above)
            InitializeManager(SceneManager.Instance);
            
            GameLogger.Info("Loading Main Menu Scene...");
            // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        private void InitializeManager(IService manager)
        {
            if (manager == null)
            {
                throw new System.Exception("A Manager instance was null during bootstrap!");
            }
            manager.Initialize();
        }
    }
}
