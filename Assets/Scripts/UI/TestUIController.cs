using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Core.Configs.Roads;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Managers;
using CityBuilder.Tools;

namespace CityBuilder.UI
{
    /// <summary>
    /// A robust IMGUI-based Test UI controller that displays menus and HUD overlays.
    /// Requires zero scene setup or Canvas dependencies.
    /// </summary>
    public class TestUIController : Singleton<TestUIController>, IService
    {
        private bool _isInitialized;
        private RoadSettings _testRoadSettings;
        private BuildingDefinition _testBuildingDef;
        private bool _showSettings;
        private string _statusMessage = "Sandbox Ready";

        public void Initialize()
        {
            if (_isInitialized) return;

            // Generate runtime mock settings to guarantee instant playability
            _testRoadSettings = ScriptableObject.CreateInstance<RoadSettings>();
            _testRoadSettings.RoadName = "Two-Lane Street";
            _testRoadSettings.RoadWidth = 8f;
            _testRoadSettings.NodeRadius = 4f;

            _testBuildingDef = ScriptableObject.CreateInstance<BuildingDefinition>();
            _testBuildingDef.BuildingID = "residential_low_01";
            _testBuildingDef.BuildingName = "Suburban House";
            _testBuildingDef.Size = new Vector2Int(2, 2);
            _testBuildingDef.BuildCost = 1000;
            _testBuildingDef.ConstructionTime = 5f;

            ServiceLocator.Register<TestUIController>(this);
            GameLogger.Info("[TestUIController] Initialized.");
            _isInitialized = true;
        }

        private void OnGUI()
        {
            if (!_isInitialized) return;

            var sm = ServiceLocator.Get<SceneManager>();
            if (sm == null) return;

            string sceneName = sm.GetActiveSceneName();

            if (sceneName == "MainMenu")
            {
                DrawMainMenu();
            }
            else if (sceneName == "Game")
            {
                DrawGameHUD();
            }
        }

        private void DrawMainMenu()
        {
            Rect menuRect = new Rect(Screen.width / 2f - 150f, Screen.height / 2f - 180f, 300f, 360f);
            GUI.Box(menuRect, "CityBuilder — Playable Prototype");

            GUILayout.BeginArea(new Rect(menuRect.x + 20f, menuRect.y + 40f, 260f, 300f));

            if (_showSettings)
            {
                GUILayout.Label("SETTINGS");
                GUILayout.Label("Graphics Quality: High");
                GUILayout.Label("Audio Volume: 100%");
                if (GUILayout.Button("Back"))
                {
                    _showSettings = false;
                }
            }
            else
            {
                if (GUILayout.Button("New Game Sandbox", GUILayout.Height(40f)))
                {
                    ServiceLocator.Get<SceneManager>()?.LoadScene("Game");
                    _statusMessage = "New game sandbox created.";
                }

                GUILayout.Space(10f);

                if (GUILayout.Button("Load AutoSave", GUILayout.Height(40f)))
                {
                    ServiceLocator.Get<SceneManager>()?.LoadScene("Game");
                    // Delay load by 1 frame to ensure restoration managers are fully initialized
                    StartCoroutine(DelayedLoadRoutine("autosave.json"));
                }

                GUILayout.Space(10f);

                if (GUILayout.Button("Settings", GUILayout.Height(40f)))
                {
                    _showSettings = true;
                }

                GUILayout.Space(10f);

                if (GUILayout.Button("Exit Sandbox", GUILayout.Height(40f)))
                {
                    Application.Quit();
                }
            }

            GUILayout.EndArea();
        }

        private System.Collections.IEnumerator DelayedLoadRoutine(string fileName)
        {
            yield return new WaitForSeconds(0.2f);
            bool success = ServiceLocator.Get<SaveManager>().LoadGame(fileName);
            _statusMessage = success ? "Autosave loaded successfully." : "Failed to load autosave.";
        }

        private void DrawGameHUD()
        {
            // 1. Top HUD
            Rect topBarRect = new Rect(10f, 10f, Screen.width - 20f, 40f);
            GUI.Box(topBarRect, "");
            GUILayout.BeginArea(new Rect(topBarRect.x + 10f, topBarRect.y + 8f, topBarRect.width - 20f, 30f));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Quit to Menu", GUILayout.Width(100f)))
            {
                ServiceLocator.Get<SceneManager>()?.LoadScene("MainMenu");
            }

            GUILayout.Space(20f);
            GUILayout.Label($"Funds: $10,000", GUILayout.Width(120f));
            GUILayout.Space(20f);
            GUILayout.Label($"Status: {_statusMessage}");
            GUILayout.FlexibleSpace();

            // Autosave Indicator (Simulation status label)
            GUILayout.Label("[AUTOSAVE ENABLED]  ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            // 2. Left Tool Selection Box
            Rect toolPanelRect = new Rect(10f, 60f, 180f, 280f);
            GUI.Box(toolPanelRect, "Editor Tools");
            GUILayout.BeginArea(new Rect(toolPanelRect.x + 10f, toolPanelRect.y + 30f, toolPanelRect.width - 20f, toolPanelRect.height - 40f));

            if (GUILayout.Button("Road Builder Tool"))
            {
                var tool = new RoadBuilderTool(_testRoadSettings);
                ServiceLocator.Get<ToolManager>()?.SetActiveTool(tool);
                _statusMessage = "Road Builder Active (Drag to build)";
            }

            GUILayout.Space(5f);

            if (GUILayout.Button("Building Placer Tool"))
            {
                ServiceLocator.Get<ToolManager>()?.ClearTool();
                ServiceLocator.Get<PlacementManager>()?.BeginPlacement(_testBuildingDef);
                _statusMessage = "Building Placer Active (Click to place, R to rotate)";
            }

            GUILayout.Space(5f);

            if (GUILayout.Button("Bulldozer Tool"))
            {
                ServiceLocator.Get<PlacementManager>()?.CancelPlacement();
                var tool = new BulldozerTool();
                ServiceLocator.Get<ToolManager>()?.SetActiveTool(tool);
                _statusMessage = "Bulldozer Active (Click to destroy)";
            }

            GUILayout.Space(5f);

            if (GUILayout.Button("Eyedropper Tool"))
            {
                var tool = new EyedropperTool();
                ServiceLocator.Get<ToolManager>()?.SetActiveTool(tool);
                _statusMessage = "Eyedropper Active (Click building to copy)";
            }

            GUILayout.Space(15f);

            if (GUILayout.Button("Clear Active Tool"))
            {
                ServiceLocator.Get<PlacementManager>()?.CancelPlacement();
                ServiceLocator.Get<ToolManager>()?.ClearTool();
                _statusMessage = "Active tool cleared.";
            }

            GUILayout.EndArea();

            // 3. Bottom Undo/Redo/Save Panel
            Rect bottomPanelRect = new Rect(10f, Screen.height - 70f, Screen.width - 20f, 60f);
            GUI.Box(bottomPanelRect, "");
            GUILayout.BeginArea(new Rect(bottomPanelRect.x + 10f, bottomPanelRect.y + 15f, bottomPanelRect.width - 20f, 40f));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Undo Action", GUILayout.Width(120f)))
            {
                ServiceLocator.Get<BuildingManager>()?.Undo();
                _statusMessage = "Undo triggered.";
            }

            if (GUILayout.Button("Redo Action", GUILayout.Width(120f)))
            {
                ServiceLocator.Get<BuildingManager>()?.Redo();
                _statusMessage = "Redo triggered.";
            }

            GUILayout.Space(40f);

            if (GUILayout.Button("Manual Save Game", GUILayout.Width(150f)))
            {
                ServiceLocator.Get<SaveManager>().SaveGame("autosave.json");
                _statusMessage = "Manual save complete.";
            }

            if (GUILayout.Button("Manual Load Game", GUILayout.Width(150f)))
            {
                bool success = ServiceLocator.Get<SaveManager>().LoadGame("autosave.json");
                _statusMessage = success ? "Load complete." : "Load failed.";
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
