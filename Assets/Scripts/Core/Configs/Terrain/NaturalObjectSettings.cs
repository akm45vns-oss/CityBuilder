using UnityEngine;
using System.Collections.Generic;

namespace CityBuilder.Core.Configs.Terrain
{
    [System.Serializable]
    public struct NaturalObjectSpawnRule
    {
        public GameObject Prefab;
        public float MinHeight;
        public float MaxHeight;
        public float MaxSlope;
        [Range(0, 1)] public float SpawnProbability;
    }

    [CreateAssetMenu(fileName = "NaturalObjectSettings", menuName = "CityBuilder/Configs/Terrain/NaturalObjectSettings")]
    public class NaturalObjectSettings : ScriptableObject
    {
        [Header("Spawning Rules")]
        public List<NaturalObjectSpawnRule> TreeRules;
        public List<NaturalObjectSpawnRule> RockRules;

        [Header("Optimization")]
        public bool UseTerrainTreeInstances = true; // If false, spawns GameObjects
    }
}
