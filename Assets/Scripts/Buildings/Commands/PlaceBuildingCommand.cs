using UnityEngine;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Grid;
using CityBuilder.Roads;
using System.Collections.Generic;

namespace CityBuilder.Buildings.Commands
{
    /// <summary>
    /// Encapsulates the action of placing a building, including grid occupancy and road connection.
    /// Undo removes the building and restores all state.
    /// </summary>
    public class PlaceBuildingCommand : IBuildingCommand
    {
        private readonly BuildingDefinition _definition;
        private readonly List<GridCell> _targetCells;
        private readonly Vector3 _position;
        private readonly int _rotationSteps;
        private readonly RoadSegment _connectedRoad;
        private readonly Vector3 _entrancePosition;

        private Building _placedBuilding;

        public PlaceBuildingCommand(
            BuildingDefinition definition,
            List<GridCell> cells,
            Vector3 position,
            int rotationSteps,
            RoadSegment connectedRoad,
            Vector3 entrancePosition)
        {
            _definition = definition;
            _targetCells = cells;
            _position = position;
            _rotationSteps = rotationSteps;
            _connectedRoad = connectedRoad;
            _entrancePosition = entrancePosition;
        }

        public bool Validate(out string reason)
        {
            reason = string.Empty;
            if (_targetCells == null || _targetCells.Count == 0)
            {
                reason = "Target footprint cells are null or empty.";
                return false;
            }

            for (int i = 0; i < _targetCells.Count; i++)
            {
                if (_targetCells[i].IsOccupied)
                {
                    reason = $"Cell ({_targetCells[i].X}, {_targetCells[i].Z}) is already occupied.";
                    return false;
                }
            }

            return true;
        }

        public void Execute()
        {
            var bm = Core.ServiceLocator.Get<Managers.BuildingManager>();
            if (bm == null) return;

            _placedBuilding = bm.PlaceBuildingInternal(
                _definition, _targetCells, _position,
                _rotationSteps * 90f, _connectedRoad, _entrancePosition);

            Core.ServiceLocator.Get<Managers.ConstructionManager>()?.AddToQueue(_placedBuilding, false);
        }

        public void Undo()
        {
            if (_placedBuilding == null) return;

            Core.ServiceLocator.Get<Managers.ConstructionManager>()?.CancelConstruction(_placedBuilding);
            Core.ServiceLocator.Get<Managers.BuildingManager>()?.RemoveBuilding(_placedBuilding.ID);

            _placedBuilding = null;
        }
    }
}
