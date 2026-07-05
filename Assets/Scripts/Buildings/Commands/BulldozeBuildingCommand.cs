using System.Collections.Generic;
using CityBuilder.Grid;

namespace CityBuilder.Buildings.Commands
{
    /// <summary>
    /// Encapsulates the action of bulldozing a building.
    /// Undo re-places the building in Completed state.
    /// </summary>
    public class BulldozeBuildingCommand : IBuildingCommand
    {
        private readonly Building _targetBuilding;

        // Snapshot of state for undo
        private readonly Core.Configs.Buildings.BuildingDefinition _definition;
        private readonly List<GridCell> _cells;
        private readonly UnityEngine.Vector3 _position;
        private readonly int _rotationSteps;
        private readonly Roads.RoadSegment _road;
        private readonly UnityEngine.Vector3 _entrance;

        private readonly string _buildingID;

        public BulldozeBuildingCommand(Building building)
        {
            _targetBuilding = building;
            _buildingID = building.ID;
            _definition = building.Definition;
            _cells = new List<GridCell>(building.OccupiedCells);
            _position = building.transform.position;
            _rotationSteps = building.RotationSteps;
            _road = building.ConnectedRoadSegment;
            _entrance = building.EntrancePosition;
        }

        public bool Validate(out string reason)
        {
            reason = string.Empty;
            var bm = Core.ServiceLocator.Get<Managers.BuildingManager>();
            if (bm == null || _targetBuilding == null || !bm.Buildings.ContainsKey(_buildingID))
            {
                reason = "Target building no longer exists or is already destroyed.";
                return false;
            }
            return true;
        }

        public void Execute()
        {
            Core.ServiceLocator.Get<Managers.ConstructionManager>()?.CancelConstruction(_targetBuilding);
            Core.ServiceLocator.Get<Managers.BuildingManager>()?.RemoveBuilding(_targetBuilding.ID);
        }

        public void Undo()
        {
            var bm = Core.ServiceLocator.Get<Managers.BuildingManager>();
            if (bm == null) return;

            // Re-free the cells only if they belong to us (BUG-006 safety check)
            for (int i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                if (cell.IsOccupied && cell.OccupyingBuildingID != _buildingID)
                {
                    GameLogger.Warning($"Cannot restore building {_buildingID} because cell ({cell.X}, {cell.Z}) is occupied by another building {cell.OccupyingBuildingID}");
                    return;
                }
                cell.Free();
            }

            var restored = bm.PlaceBuildingInternal(_definition, _cells, _position, _rotationSteps * 90f, _road, _entrance);
            restored.FinishConstruction(); // Skip construction timer on undo
        }
    }
}
