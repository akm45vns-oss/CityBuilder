using UnityEngine;
using System.Collections.Generic;

namespace CityBuilder.Core.Configs.Buildings
{
    public enum BuildingCategory
    {
        Residential,
        Commercial,
        Industrial,
        Office,
        Government,
        Healthcare,
        Education,
        Police,
        Fire,
        Power,
        Water,
        Waste,
        Transportation,
        Parks,
        Decoration,
        Unique,
        Landmark
    }

    [System.Serializable]
    public struct UpgradeLevel
    {
        public string UpgradeName;
        public int Cost;
        public int AdditionalWorkers;
        public int AdditionalResidents;
        public int AdditionalPower;
        public GameObject UpgradedPrefab;
    }

    [System.Serializable]
    public struct UnlockRequirement
    {
        public string RequiredBuildingID;
        public int RequiredPopulation;
    }

    /// <summary>
    /// Full data definition for a single building type.
    /// This is the single source of truth for a building's identity and simulation specs.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingDefinition", menuName = "CityBuilder/Buildings/BuildingDefinition")]
    public class BuildingDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string BuildingID;
        public string BuildingName;
        [TextArea(2, 4)] public string Description;
        public BuildingCategory Category;
        public Sprite Thumbnail;

        [Header("Footprint")]
        public Vector2Int Size = new Vector2Int(1, 1);
        /// <summary>
        /// Custom irregular cell offsets from the anchor cell. If empty, uses Size as a rectangle.
        /// </summary>
        public List<Vector2Int> CustomFootprint;

        [Header("Prefabs")]
        public GameObject MainPrefab;
        public GameObject ConstructionPrefab;
        public List<GameObject> LODPrefabs;

        [Header("Economy")]
        public int BuildCost = 1000;
        public int MaintenanceCost = 50;
        public float ConstructionTime = 15f;

        [Header("Simulation: Workers & Residents")]
        public int MaxWorkers = 0;
        public int MaxResidents = 0;

        [Header("Simulation: Utilities")]
        public int ElectricityConsumption = 10;
        public int WaterConsumption = 10;
        public int SewageProduction = 8;
        public int GarbageProduction = 5;

        [Header("Simulation: Environmental")]
        [Range(0, 100)] public int Pollution = 0;
        [Range(0, 100)] public int Noise = 10;
        [Range(0, 100)] public int FireRisk = 10;

        [Header("Access")]
        public bool RequiresRoadAccess = true;

        [Header("Upgrades")]
        public List<UpgradeLevel> UpgradeLevels;

        [Header("Unlock Conditions")]
        public UnlockRequirement UnlockRequirement;

        private List<Vector2Int> _cachedRectFootprint;

        /// <summary>
        /// Returns the footprint as a list of cell offsets from the origin.
        /// Falls back to rectangular Size if no custom footprint is defined.
        /// </summary>
        public List<Vector2Int> GetFootprintOffsets()
        {
            if (CustomFootprint != null && CustomFootprint.Count > 0)
                return CustomFootprint;

            if (_cachedRectFootprint != null) return _cachedRectFootprint;

            _cachedRectFootprint = new List<Vector2Int>();
            for (int x = 0; x < Size.x; x++)
                for (int z = 0; z < Size.y; z++)
                    _cachedRectFootprint.Add(new Vector2Int(x, z));
            return _cachedRectFootprint;
        }
    }
}
