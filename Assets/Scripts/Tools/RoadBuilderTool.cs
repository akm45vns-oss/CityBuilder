using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Configs.Roads;
using CityBuilder.Managers;
using CityBuilder.Grid;

namespace CityBuilder.Tools
{
    public class RoadBuilderTool : ITool
    {
        private RoadSettings _settings;
        private Vector3 _startPos;
        private bool _isDragging;

        public RoadBuilderTool(RoadSettings settings)
        {
            _settings = settings;
        }

        public void OnEnable()
        {
            _isDragging = false;
        }

        public void OnDisable()
        {
            _isDragging = false;
        }

        public void OnUpdate()
        {
            var input = ServiceLocator.Get<InputManager>();
            if (input == null || UnityEngine.Camera.main == null) return;

            // Simplified implementation for demonstration
            // Real implementation would raycast against terrain and snap to GridCells or existing RoadNodes
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input.GetMousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (input.SelectAction.WasPressedThisFrame())
                {
                    _startPos = hit.point;
                    _isDragging = true;
                }
                else if (input.SelectAction.WasReleasedThisFrame() && _isDragging)
                {
                    _isDragging = false;
                    float dist = Vector3.Distance(_startPos, hit.point);
                    if (dist > _settings.NodeRadius * 2)
                    {
                        var rm = ServiceLocator.Get<RoadManager>();
                        rm?.BuildRoad(_startPos, hit.point, _settings);
                    }
                }
            }
        }
    }
}
