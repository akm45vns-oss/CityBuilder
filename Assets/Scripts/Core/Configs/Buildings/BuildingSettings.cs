using UnityEngine;

namespace CityBuilder.Core.Configs.Buildings
{
    public enum BuildingCategory
    {
        Residential,
        Commercial,
        Industrial,
        Office,
        Decoration,
        Park,
        Service,
        Utility,
        RoadDecoration
    }

    [CreateAssetMenu(fileName = "BuildingSettings", menuName = "CityBuilder/Configs/Buildings/BuildingSettings")]
    public class BuildingSettings : ScriptableObject
    {
        [Header("Identity")]
        public string BuildingName;
        public BuildingCategory Category;
        public GameObject Prefab;
        public GameObject ConstructionPrefab; // Ghost or scaffolding

        [Header("Footprint")]
        public Vector2Int Size = new Vector2Int(1, 1); // In grid cells
        public bool RequiresRoadAccess = true;

        [Header("Economy")]
        public int BuildCost = 1000;
        public int MaintenanceCost = 50;
        public float ConstructionTime = 10f; // seconds

        [Header("Simulation Specs")]
        public int MaxWorkers = 0;
        public int MaxResidents = 0;
        public int PowerUsage = 10;
        public int WaterUsage = 10;
        public int GarbageOutput = 5;

        [Header("Services")]
        public int EducationNeed = 0;
        public int FireRisk = 10;
    }
}
