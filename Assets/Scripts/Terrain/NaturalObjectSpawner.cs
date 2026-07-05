using UnityEngine;
using CityBuilder.Core.Configs.Terrain;
using CityBuilder.Core.Logging;

namespace CityBuilder.Terrain
{
    /// <summary>
    /// Handles procedural spawning of trees and rocks utilizing Unity's Terrain Tree Instances.
    /// This is highly optimized for rendering 100,000+ trees.
    /// </summary>
    public class NaturalObjectSpawner
    {
        public static void Spawn(UnityEngine.Terrain terrain, NaturalObjectSettings settings)
        {
            if (terrain == null || settings == null || settings.TreeRules.Count == 0) return;

            TerrainData tData = terrain.terrainData;
            
            // Map prefabs to Terrain TreePrototypes
            TreePrototype[] prototypes = new TreePrototype[settings.TreeRules.Count];
            for (int i = 0; i < settings.TreeRules.Count; i++)
            {
                prototypes[i] = new TreePrototype { prefab = settings.TreeRules[i].Prefab };
            }
            tData.treePrototypes = prototypes;

            // Generate Instances
            var instances = new System.Collections.Generic.List<TreeInstance>();
            int resolution = tData.heightmapResolution;

            for (int x = 0; x < resolution; x += 5) // Step size controls max density
            {
                for (int z = 0; z < resolution; z += 5)
                {
                    float normX = (float)x / resolution;
                    float normZ = (float)z / resolution;

                    float height = tData.GetHeight(x, z);
                    float slope = tData.GetSteepness(normX, normZ);

                    // Pick a random rule
                    int ruleIndex = Random.Range(0, settings.TreeRules.Count);
                    var rule = settings.TreeRules[ruleIndex];

                    if (height >= rule.MinHeight && height <= rule.MaxHeight && slope <= rule.MaxSlope)
                    {
                        if (Random.value <= rule.SpawnProbability)
                        {
                            TreeInstance inst = new TreeInstance
                            {
                                position = new Vector3(normX, 0, normZ),
                                prototypeIndex = ruleIndex,
                                widthScale = Random.Range(0.8f, 1.2f),
                                heightScale = Random.Range(0.8f, 1.2f),
                                color = Color.white,
                                lightmapColor = Color.white
                            };
                            instances.Add(inst);
                        }
                    }
                }
            }

            tData.SetTreeInstances(instances.ToArray(), true);
            GameLogger.Info($"[NaturalObjectSpawner] Spawned {instances.Count} tree instances.");
        }
    }
}
