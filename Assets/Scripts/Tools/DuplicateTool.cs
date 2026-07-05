using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Managers;
using CityBuilder.Buildings.Commands;

namespace CityBuilder.Tools
{
    /// <summary>
    /// Clicks an existing building and executes a duplicate placement at the clicked position.
    /// Uses the same Command pattern as normal placement for full Undo/Redo support.
    /// </summary>
    public class DuplicateTool : ITool
    {
        public void OnEnable() { }
        public void OnDisable() { }

        public void OnUpdate()
        {
            var input = ServiceLocator.Get<InputManager>();
            if (input == null || UnityEngine.Camera.main == null) return;

            if (input.SelectAction.WasPressedThisFrame())
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input.GetMousePosition());
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var building = hit.collider.GetComponentInParent<Buildings.Building>();
                    if (building != null && building.Definition != null)
                    {
                        // Begin placement with the same definition and immediately queue it
                        var pm = ServiceLocator.Get<PlacementManager>();
                        pm?.BeginPlacement(building.Definition);

                        ServiceLocator.Get<ToolManager>()?.ClearTool();
                    }
                }
            }
        }
    }
}
