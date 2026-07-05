using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Tools;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Routes gameplay input to the currently active tool (e.g. Bulldozer, RoadBuilder).
    /// </summary>
    public class ToolManager : Singleton<ToolManager>, IService
    {
        private bool _isInitialized;
        private ITool _currentTool;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            ServiceLocator.Register<ToolManager>(this);
            GameLogger.Info("[ToolManager] Initialized.");
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            _currentTool?.OnUpdate();
        }

        public void SetActiveTool(ITool tool)
        {
            if (_currentTool == tool) return;

            _currentTool?.OnDisable();
            _currentTool = tool;
            _currentTool?.OnEnable();

            GameLogger.Verbose($"[ToolManager] Switched tool to {tool?.GetType().Name ?? "None"}");
        }

        public void ClearTool()
        {
            SetActiveTool(null);
        }
    }
}
