using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Development-only manager for profiling, FPS counting, and debugging tools.
    /// </summary>
    public class DebugManager : Singleton<DebugManager>, IService
    {
        private bool _isInitialized;
        private float _deltaTime;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<DebugManager>(this);
            GameLogger.Info("[DebugManager] Initialized.");
            _isInitialized = true;
        }

        private void Update()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
#endif
        }

        private void OnGUI()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!ServiceLocator.Get<ConfigManager>()) return; // Wait for config

            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            
            // FPS
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            GUILayout.Label(string.Format("FPS: {0:0.0} ({1:0.0} ms)", fps, msec));

            // Camera Data
            var cam = ServiceLocator.Get<CityBuilder.Camera.CameraManager>();
            if (cam != null)
            {
                GUILayout.Label($"Cam Pos: {cam.CameraRoot.position}");
                GUILayout.Label($"Cam Mode: {cam.GetCurrentState()?.GetType().Name}");
            }

            // Selection Data
            var sel = ServiceLocator.Get<SelectionManager>();
            if (sel != null && sel.CurrentSelection != null)
            {
                GUILayout.Label($"Selected: {sel.CurrentSelection.GetName()}");
            }

            // Terrain / Grid Data
            var tm = ServiceLocator.Get<TerrainManager>();
            if (tm != null && tm.Grid != null)
            {
                GUILayout.Label($"Grid: {tm.Grid.Width}x{tm.Grid.Height}");
            }
            
            // City Data
            var rm = ServiceLocator.Get<RoadManager>();
            if (rm != null && rm.Network != null)
            {
                GUILayout.Label($"Road Nodes: {rm.Network.Nodes.Count}");
                GUILayout.Label($"Road Segments: {rm.Network.Segments.Count}");
            }
            
            var bm = ServiceLocator.Get<BuildingManager>();
            if (bm != null)
            {
                GUILayout.Label($"Total Buildings: {bm.Buildings.Count}");
            }

            GUILayout.EndArea();
#endif
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!Application.isPlaying) return;

            // 1. Draw Grid
            var tm = ServiceLocator.Get<TerrainManager>();
            if (tm != null && tm.Grid != null && tm.GridSettings != null && UnityEngine.Camera.main != null)
            {
                Vector3 camPos = UnityEngine.Camera.main.transform.position;
                var centerCell = tm.Grid.GetCellFromWorldPosition(camPos, tm.ActiveTerrain);
                if (centerCell != null)
                {
                    int drawRadius = 10;
                    float size = tm.GridSettings.CellSize;
                    for (int x = Mathf.Max(0, centerCell.X - drawRadius); x < Mathf.Min(tm.Grid.Width, centerCell.X + drawRadius); x++)
                    {
                        for (int z = Mathf.Max(0, centerCell.Z - drawRadius); z < Mathf.Min(tm.Grid.Height, centerCell.Z + drawRadius); z++)
                        {
                            var cell = tm.Grid.GetCell(x, z);
                            Gizmos.color = cell.IsBuildable ? new Color(0, 1, 0, 0.1f) : new Color(1, 0, 0, 0.1f);
                            if (cell.IsOccupied) Gizmos.color = new Color(0, 0, 1, 0.3f);
                            Gizmos.DrawCube(cell.WorldPosition, new Vector3(size - 0.1f, 0.1f, size - 0.1f));
                        }
                    }
                }
            }
            
            // 2. Draw Road Graph Mathematical Model
            var rm = ServiceLocator.Get<RoadManager>();
            if (rm != null && rm.Network != null)
            {
                foreach (var node in rm.Network.Nodes.Values)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(node.Position, 0.5f);
                }
                
                foreach (var segment in rm.Network.Segments.Values)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(segment.StartNode.Position, segment.EndNode.Position);
                }
            }
#endif
        }
    }
}
