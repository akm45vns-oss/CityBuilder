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

        public BulldozeBuildingCommand(Building building)
        {
            _targetBuilding = building;
            _definition = building.Definition;
            _cells = new List<GridCell>(building.OccupiedCells);
            _position = building.transform.position;
            _rotationSteps = building.RotationSteps;
            _road = building.ConnectedRoadSegment;
            _entrance = building.EntrancePosition;
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

            // Re-free the cells since they may have been re-occupied
            foreach (var c in _cells) c.Free();

            var restored = bm.PlaceBuildingInternal(_definition, _cells, _position, _rotationSteps * 90f, _road, _entrance);
            restored.FinishConstruction(); // Skip construction timer on undo
        }
    }
}
