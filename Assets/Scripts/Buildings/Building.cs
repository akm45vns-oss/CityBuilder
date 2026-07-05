using UnityEngine;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Core.Selection;
using System.Collections.Generic;
using CityBuilder.Grid;
using CityBuilder.Roads;

namespace CityBuilder.Buildings
{
    public enum BuildingState
    {
        Blueprint,
        UnderConstruction,
        Completed,
        Disabled,
        Abandoned
    }

    /// <summary>
    /// Runtime instance of a placed building.
    /// Stores live simulation data, state, and road connection.
    /// </summary>
    public class Building : MonoBehaviour, ISelectable
    {
        public string ID { get; private set; }
        public BuildingDefinition Definition { get; private set; }
        public BuildingState State { get; private set; }

        public List<GridCell> OccupiedCells { get; private set; } = new List<GridCell>();
        public int RotationSteps { get; private set; }

        // Road connection — prepared for citizen pathfinding
        public RoadSegment ConnectedRoadSegment { get; private set; }
        public Vector3 EntrancePosition { get; private set; }

        // Live simulation data
        public int CurrentWorkers { get; set; }
        public int CurrentResidents { get; set; }
        public int CurrentUpgradeLevel { get; private set; }

        private GameObject _modelInstance;

        public void Initialize(BuildingDefinition definition, List<GridCell> cells, int rotationSteps)
        {
            ID = System.Guid.NewGuid().ToString();
            Definition = definition;
            OccupiedCells = cells;
            RotationSteps = rotationSteps;
            State = BuildingState.Blueprint;

            // Mark cells occupied
            float rotDegrees = rotationSteps * 90f;
            foreach (var cell in OccupiedCells)
            {
                cell.Occupy(ID, rotDegrees);
            }
        }

        public void SetRoadConnection(RoadSegment segment, Vector3 entrancePos)
        {
            ConnectedRoadSegment = segment;
            EntrancePosition = entrancePos;

            // Tag entrance cell for pathfinding
            var nearestCell = OccupiedCells.Count > 0 ? OccupiedCells[0] : null;
            if (nearestCell != null) nearestCell.HasRoadConnection = true;
        }

        public void BeginConstruction()
        {
            State = BuildingState.UnderConstruction;
            SwapModel(Definition.ConstructionPrefab);
        }

        public void FinishConstruction()
        {
            State = BuildingState.Completed;
            SwapModel(Definition.MainPrefab);
        }

        public void Disable()
        {
            State = BuildingState.Disabled;
        }

        public void SetAbandoned()
        {
            State = BuildingState.Abandoned;
        }

        public void UpgradeTo(int level)
        {
            if (Definition.UpgradeLevels == null || level >= Definition.UpgradeLevels.Count) return;
            CurrentUpgradeLevel = level;
            var upgrade = Definition.UpgradeLevels[level];
            if (upgrade.UpgradedPrefab != null)
            {
                SwapModel(upgrade.UpgradedPrefab);
            }
        }

        /// <summary>
        /// Destroys this building and frees all occupied grid cells.
        /// </summary>
        public void DestroyBuilding()
        {
            foreach (var cell in OccupiedCells)
            {
                cell.Free();
            }
            Destroy(gameObject);
        }

        private void SwapModel(GameObject prefab)
        {
            if (_modelInstance != null) Destroy(_modelInstance);
            if (prefab != null)
            {
                _modelInstance = Instantiate(prefab, transform.position, transform.rotation, transform);
            }
        }

        // ISelectable
        public string GetName() => Definition != null ? Definition.BuildingName : "Unknown Building";
        public void OnSelected()
        {
            // Future: Show UI info panel, highlight outline
        }
        public void OnDeselected()
        {
            // Future: Hide outline, close panel
        }
    }
}
