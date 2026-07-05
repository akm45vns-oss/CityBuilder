using UnityEngine;
using UnityEngine.InputSystem;
using CityBuilder.Core;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Buildings;
using CityBuilder.Buildings.Commands;
using CityBuilder.Grid;
using System.Collections.Generic;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Decoupled manager for ghost preview positioning, grid snapping, and executing placement.
    /// This is the bridge between the UI/Input and the BuildingManager.
    /// </summary>
    public class PlacementManager : Singleton<PlacementManager>, IService
    {
        private bool _isInitialized;

        private BuildingDefinition _activeDefinition;
        private int _rotationSteps;
        private GameObject _ghostInstance;

        private bool _isValid;
        private List<GridCell> _previewCells;
        private Roads.RoadSegment _previewRoad;
        private Vector3 _previewEntrance;

        // Ghost tint materials
        [Header("Ghost Materials")]
        public Material ValidGhostMaterial;
        public Material InvalidGhostMaterial;

        public void Initialize()
        {
            if (_isInitialized) return;
            ServiceLocator.Register<PlacementManager>(this);
            GameLogger.Info("[PlacementManager] Initialized.");
            _isInitialized = true;
        }

        public void BeginPlacement(BuildingDefinition definition)
        {
            CancelPlacement();
            _activeDefinition = definition;
            _rotationSteps = 0;

            if (definition.ConstructionPrefab != null)
            {
                _ghostInstance = Instantiate(definition.ConstructionPrefab);
            }
        }

        public void CancelPlacement()
        {
            if (_ghostInstance != null) Destroy(_ghostInstance);
            _activeDefinition = null;
            _previewCells = null;
            _isValid = false;
        }

        private void Update()
        {
            if (!_isInitialized || _activeDefinition == null || _ghostInstance == null) return;

            var input = ServiceLocator.Get<InputManager>();
            var tm = ServiceLocator.Get<TerrainManager>();
            if (input == null || tm == null || tm.Grid == null || UnityEngine.Camera.main == null) return;

            // Rotate (R key)
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                _rotationSteps = (_rotationSteps + 1) % 4;
                _ghostInstance.transform.rotation = Quaternion.Euler(0, _rotationSteps * 90f, 0);
            }

            // Undo (Ctrl+Z)
            if (Keyboard.current.ctrlKey.isPressed && Keyboard.current.zKey.wasPressedThisFrame)
            {
                ServiceLocator.Get<BuildingManager>()?.Undo();
            }

            // Redo (Ctrl+Y)
            if (Keyboard.current.ctrlKey.isPressed && Keyboard.current.yKey.wasPressedThisFrame)
            {
                ServiceLocator.Get<BuildingManager>()?.Redo();
            }

            // Raycast
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input.GetMousePosition());
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var cell = tm.Grid.GetCellFromWorldPosition(hit.point, tm.ActiveTerrain);
            if (cell == null) return;

            _ghostInstance.transform.position = cell.WorldPosition;

            // Validate
            var error = PlacementValidator.Validate(
                tm.Grid, cell.X, cell.Z, _activeDefinition, _rotationSteps,
                out _previewCells, out _previewRoad, out _previewEntrance);

            _isValid = (error == PlacementError.None);

            // Tint ghost based on validity
            TintGhost(_isValid);

            // Place on click
            if (_isValid && input.SelectAction.WasPressedThisFrame())
            {
                var cmd = new PlaceBuildingCommand(
                    _activeDefinition, _previewCells, cell.WorldPosition,
                    _rotationSteps, _previewRoad, _previewEntrance);

                ServiceLocator.Get<BuildingManager>()?.ExecuteCommand(cmd);
            }
        }

        private void TintGhost(bool valid)
        {
            if (_ghostInstance == null) return;
            Material mat = valid ? ValidGhostMaterial : InvalidGhostMaterial;
            if (mat == null) return;

            foreach (var renderer in _ghostInstance.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterial = mat;
            }
        }
    }
}
