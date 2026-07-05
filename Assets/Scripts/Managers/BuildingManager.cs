using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Buildings.Commands;
using CityBuilder.Roads;
using System.Collections.Generic;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Authoritative manager for all building placement.
    /// Implements a full Undo/Redo command history.
    /// </summary>
    public class BuildingManager : Singleton<BuildingManager>, IService
    {
        private bool _isInitialized;

        public Dictionary<string, Buildings.Building> Buildings { get; private set; }
            = new Dictionary<string, Buildings.Building>();

        private Stack<IBuildingCommand> _undoStack = new Stack<IBuildingCommand>();
        private Stack<IBuildingCommand> _redoStack = new Stack<IBuildingCommand>();

        public void Initialize()
        {
            if (_isInitialized) return;
            ServiceLocator.Register<BuildingManager>(this);
            GameLogger.Info("[BuildingManager] Initialized.");
            _isInitialized = true;
        }

        // ─── Command Execution ────────────────────────────────────────────────

        public void ExecuteCommand(IBuildingCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Branching clears redo history
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            var cmd = _undoStack.Pop();
            cmd.Undo();
            _redoStack.Push(cmd);
            GameLogger.Verbose("[BuildingManager] Undo.");
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            var cmd = _redoStack.Pop();
            cmd.Execute();
            _undoStack.Push(cmd);
            GameLogger.Verbose("[BuildingManager] Redo.");
        }

        // ─── Internal Placement (called by Commands only) ─────────────────────

        public Buildings.Building PlaceBuildingInternal(
            BuildingDefinition definition,
            List<Grid.GridCell> cells,
            Vector3 position,
            float rotationDegrees,
            RoadSegment connectedRoad,
            Vector3 entrancePosition)
        {
            GameObject go = new GameObject($"Building_{definition.BuildingID}");
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(0, rotationDegrees, 0);
            go.transform.SetParent(transform);

            // Box collider for raycasting
            BoxCollider col = go.AddComponent<BoxCollider>();
            col.size = new Vector3(definition.Size.x * 8f, 10f, definition.Size.y * 8f);
            col.center = new Vector3(0, 5f, 0);

            Buildings.Building building = go.AddComponent<Buildings.Building>();
            int rotSteps = Mathf.RoundToInt(rotationDegrees / 90f);
            building.Initialize(definition, cells, rotSteps);
            building.SetRoadConnection(connectedRoad, entrancePosition);

            Buildings.Add(building.ID, building);
            GameLogger.Verbose($"[BuildingManager] Placed: {definition.BuildingName}");
            return building;
        }

        public void RemoveBuilding(string id)
        {
            if (Buildings.TryGetValue(id, out var building))
            {
                building.DestroyBuilding();
                Buildings.Remove(id);
                GameLogger.Verbose($"[BuildingManager] Removed building {id}");
            }
        }
    }
}
