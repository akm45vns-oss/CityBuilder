using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Managers;
using CityBuilder.Buildings;
using System.Collections.Generic;

namespace CityBuilder.Tools
{
    public class BuildingPlacerTool : ITool
    {
        private BuildingSettings _settings;
        private GameObject _ghost;
        private float _currentRotation;

        public BuildingPlacerTool(BuildingSettings settings)
        {
            _settings = settings;
        }

        public void OnEnable()
        {
            _currentRotation = 0f;
            if (_settings.ConstructionPrefab != null)
            {
                _ghost = Object.Instantiate(_settings.ConstructionPrefab);
            }
        }

        public void OnDisable()
        {
            if (_ghost != null) Object.Destroy(_ghost);
        }

        public void OnUpdate()
        {
            var input = ServiceLocator.Get<InputManager>();
            if (input == null || UnityEngine.Camera.main == null || _ghost == null) return;

            // Rotate
            if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
            {
                _currentRotation += 90f;
            }

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input.GetMousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Snap to grid
                var tm = ServiceLocator.Get<TerrainManager>();
                if (tm != null && tm.Grid != null)
                {
                    var cell = tm.Grid.GetCellFromWorldPosition(hit.point, tm.ActiveTerrain);
                    if (cell != null)
                    {
                        _ghost.transform.position = cell.WorldPosition;
                        _ghost.transform.rotation = Quaternion.Euler(0, _currentRotation, 0);

                        var error = PlacementValidator.Validate(cell.WorldPosition, _settings, _currentRotation, out List<Grid.GridCell> targetCells);
                        
                        // Tint ghost based on validity (assuming ghost has material)
                        // True implementation needs recursive material search
                        
                        if (error == PlacementError.None && input.SelectAction.WasPressedThisFrame())
                        {
                            var bm = ServiceLocator.Get<BuildingManager>();
                            var building = bm?.PlaceBuilding(cell.WorldPosition, _ghost.transform.rotation, _settings, targetCells);
                            
                            if (building != null)
                            {
                                ServiceLocator.Get<ConstructionManager>()?.AddToQueue(building, false);
                            }
                        }
                    }
                }
            }
        }
    }
}
