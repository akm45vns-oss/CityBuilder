using UnityEngine;

namespace CityBuilder.Core.Configs.Roads
{
    public enum RoadCategory
    {
        Dirt,
        Small,
        Medium,
        Large,
        Highway,
        Pedestrian
    }

    [CreateAssetMenu(fileName = "RoadSettings", menuName = "CityBuilder/Configs/Roads/RoadSettings")]
    public class RoadSettings : ScriptableObject
    {
        [Header("Classification")]
        public string RoadName;
        public RoadCategory Category;
        public int Lanes = 2;
        public bool IsOneWay = false;

        [Header("Geometry")]
        public float RoadWidth = 6f;
        public float NodeRadius = 3f;
        public Material RoadMaterial;
        public Material IntersectionMaterial;

        [Header("Gameplay Data")]
        public float SpeedLimit = 40f; // km/h
        public int BuildCostPerUnit = 10;
        public int MaintenanceCostPerUnit = 1;

        [Header("Future Features")]
        public bool SupportsTrams = false;
        public bool SupportsBikes = false;
        public bool SupportsBuses = false;
    }
}
