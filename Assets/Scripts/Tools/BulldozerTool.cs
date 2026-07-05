using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Managers;
using CityBuilder.Buildings.Commands;

namespace CityBuilder.Tools
{
    public class BulldozerTool : ITool
    {
        public void OnEnable()
        {
            // Future: show bulldozer cursor or overlay tint
        }

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
                    if (building != null)
                    {
                        var cmd = new BulldozeBuildingCommand(building);
                        ServiceLocator.Get<BuildingManager>()?.ExecuteCommand(cmd);
                        return;
                    }

                    // Future: bulldoze roads via RoadManager
                }
            }
        }
    }
}
