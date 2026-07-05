using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Managers;

namespace CityBuilder.Tools
{
    /// <summary>
    /// Clicks an existing building and automatically activates placement mode
    /// with the same definition — allowing immediate repeat placement.
    /// </summary>
    public class EyedropperTool : ITool
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
                        var pm = ServiceLocator.Get<PlacementManager>();
                        pm?.BeginPlacement(building.Definition);

                        // Switch to the placement manager tool
                        ServiceLocator.Get<ToolManager>()?.ClearTool();
                    }
                }
            }
        }
    }
}
