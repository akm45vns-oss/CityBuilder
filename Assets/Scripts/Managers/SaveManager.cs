using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.SaveSystem;
using CityBuilder.Roads;
using CityBuilder.Buildings;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles saving, loading, and serializing game state data using versioned DTOs and migrations.
    /// </summary>
    public class SaveManager : Singleton<SaveManager>, IService
    {
        private bool _isInitialized;
        private string _savePath;

        public void Initialize()
        {
            if (_isInitialized) return;

            _savePath = Path.Combine(Application.persistentDataPath, "saves");
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            ServiceLocator.Register<SaveManager>(this);
            GameLogger.Info($"[SaveManager] Initialized. Save path: {_savePath}");
            _isInitialized = true;
        }

        public void SaveGame(string fileName)
        {
            try
            {
                SaveDataDTO dto = CreateSaveDTO();
                string json = JsonUtility.ToJson(dto, true);
                
                // Add Checksum integrity validation
                string checksum = SaveMigrationSystem.ComputeChecksum(json);
                dto.Checksum = checksum;
                
                // Re-serialize with checksum included
                string finalJson = JsonUtility.ToJson(dto, true);
                string fullPath = Path.Combine(_savePath, fileName);
                
                File.WriteAllText(fullPath, finalJson);
                GameLogger.Info($"[SaveManager] Saved city state to: {fullPath}");
            }
            catch (Exception e)
            {
                GameLogger.Exception(e);
                GameLogger.Error("[SaveManager] Critical error during save sequence.");
            }
        }

        public bool LoadGame(string fileName)
        {
            string fullPath = Path.Combine(_savePath, fileName);
            if (!File.Exists(fullPath))
            {
                GameLogger.Error($"[SaveManager] Save file not found: {fullPath}");
                return false;
            }

            try
            {
                string rawJson = File.ReadAllText(fullPath);

                // Quick deserialize of meta headers to verify version and check checksum
                SaveDataDTO header = JsonUtility.FromJson<SaveDataDTO>(rawJson);
                
                // Checksum integrity verification
                string savedChecksum = header.Checksum;
                header.Checksum = string.Empty; // Zero out checksum field for matching check
                string checkJson = JsonUtility.ToJson(header, true);

                if (!SaveMigrationSystem.VerifyIntegrity(checkJson, savedChecksum))
                {
                    GameLogger.Warning("[SaveManager] Save file checksum mismatch! The file might be corrupted.");
                }

                // Handle version upgrades via migration pipeline
                SaveDataDTO migratedDTO = SaveMigrationSystem.Migrate(rawJson, header.SaveVersion);

                RestoreFromDTO(migratedDTO);
                GameLogger.Info($"[SaveManager] Loaded and restored city state from: {fullPath}");
                return true;
            }
            catch (Exception e)
            {
                GameLogger.Exception(e);
                GameLogger.Error("[SaveManager] Critical error during load sequence. Restoring fallback...");
                return false;
            }
        }

        private SaveDataDTO CreateSaveDTO()
        {
            SaveDataDTO dto = new SaveDataDTO
            {
                SaveVersion = SaveMigrationSystem.CurrentVersion,
                Timestamp = DateTime.UtcNow.ToString("o"),
                RoadNetworkData = new RoadNetworkSaveDTO(),
                BuildingData = new BuildingSaveDTO(),
                ConstructionData = new ConstructionSaveDTO()
            };

            // 1. Serialize Roads
            var rm = ServiceLocator.Get<RoadManager>();
            if (rm != null && rm.Network != null)
            {
                foreach (var node in rm.Network.Nodes.Values)
                {
                    dto.RoadNetworkData.Nodes.Add(new RoadNodeDTO { ID = node.ID, Position = node.Position });
                }

                foreach (var segment in rm.Network.Segments.Values)
                {
                    dto.RoadNetworkData.Segments.Add(new RoadSegmentDTO
                    {
                        ID = segment.ID,
                        StartNodeID = segment.StartNode.ID,
                        EndNodeID = segment.EndNode.ID,
                        SettingsName = segment.Settings != null ? segment.Settings.name : string.Empty,
                        SplinePoints = new List<Vector3>(segment.SplinePoints)
                    });
                }
            }

            // 2. Serialize Buildings
            var bm = ServiceLocator.Get<BuildingManager>();
            if (bm != null)
            {
                foreach (var b in bm.Buildings.Values)
                {
                    dto.BuildingData.Instances.Add(new BuildingInstanceDTO
                    {
                        ID = b.ID,
                        DefinitionID = b.Definition != null ? b.Definition.BuildingID : string.Empty,
                        Position = b.Position,
                        RotationSteps = b.RotationSteps,
                        State = b.State.ToString(),
                        ConnectedRoadSegmentID = b.ConnectedRoadSegment != null ? b.ConnectedRoadSegment.ID : string.Empty,
                        EntrancePosition = b.EntrancePosition,
                        CurrentWorkers = b.CurrentWorkers,
                        CurrentResidents = b.CurrentResidents,
                        CurrentUpgradeLevel = b.CurrentUpgradeLevel
                    });
                }
            }

            // 3. Serialize Construction Queue
            var cm = ServiceLocator.Get<ConstructionManager>();
            if (cm != null)
            {
                foreach (var job in cm.ActiveJobs)
                {
                    dto.ConstructionData.ActiveJobs.Add(new ConstructionJobDTO
                    {
                        BuildingID = job.TargetBuilding.ID,
                        TotalTime = job.TotalTime,
                        TimeRemaining = job.TimeRemaining,
                        IsPaused = job.IsPaused
                    });
                }
            }

            return dto;
        }

        private void RestoreFromDTO(SaveDataDTO dto)
        {
            // Clear current state first
            var rm = ServiceLocator.Get<RoadManager>();
            var bm = ServiceLocator.Get<BuildingManager>();
            var cm = ServiceLocator.Get<ConstructionManager>();
            var tm = ServiceLocator.Get<TerrainManager>();

            if (rm != null)
            {
                rm.Network.Clear();
                rm.MeshGenerator.transform.DetachChildren(); // Simple clean
            }
            if (bm != null)
            {
                bm.Buildings.Clear();
            }
            if (cm != null)
            {
                cm.CancelAllJobs(); // Exposes clean method
            }

            // Reset grid occupancy
            if (tm != null && tm.Grid != null)
            {
                for (int x = 0; x < tm.Grid.Width; x++)
                    for (int z = 0; z < tm.Grid.Height; z++)
                        tm.Grid.GetCell(x, z).Free();
            }

            // 1. Reconstruct Roads
            Dictionary<string, RoadNode> nodeLookup = new Dictionary<string, RoadNode>();
            if (dto.RoadNetworkData != null)
            {
                foreach (var nDTO in dto.RoadNetworkData.Nodes)
                {
                    var node = rm.Network.AddNode(nDTO.Position);
                    nodeLookup[nDTO.ID] = node;
                }

                foreach (var sDTO in dto.RoadNetworkData.Segments)
                {
                    if (nodeLookup.TryGetValue(sDTO.StartNodeID, out var start) &&
                        nodeLookup.TryGetValue(sDTO.EndNodeID, out var end))
                    {
                        // Find settings from ConfigManager or Resource
                        var settings = Resources.Load<Core.Configs.Roads.RoadSettings>($"Roads/{sDTO.SettingsName}");
                        var segment = rm.Network.AddSegment(start, end, settings, sDTO.SplinePoints);
                        rm.MeshGenerator.RebuildMesh(segment);
                    }
                }
            }

            // 2. Reconstruct Buildings
            Dictionary<string, Building> buildingLookup = new Dictionary<string, Building>();
            if (dto.BuildingData != null)
            {
                foreach (var bDTO in dto.BuildingData.Instances)
                {
                    var def = Resources.Load<Core.Configs.Buildings.BuildingDefinition>($"Buildings/{bDTO.DefinitionID}");
                    if (def == null) continue;

                    // Resolve target cells from footprint
                    int anchorX = Mathf.RoundToInt(bDTO.Position.x / 8f); // Simplified cell mapping
                    int anchorZ = Mathf.RoundToInt(bDTO.Position.z / 8f);
                    
                    var cells = FootprintManager.ResolveCells(tm.Grid, anchorX, anchorZ, def, bDTO.RotationSteps);
                    if (cells == null) continue;

                    var road = rm.Network.Segments.TryGetValue(bDTO.ConnectedRoadSegmentID, out var seg) ? seg : null;

                    var building = bm.PlaceBuildingInternal(def, cells, bDTO.Position, bDTO.RotationSteps * 90f, road, bDTO.EntrancePosition);
                    buildingLookup[bDTO.ID] = building;

                    // Restore state
                    if (Enum.TryParse<BuildingState>(bDTO.State, out var state))
                    {
                        if (state == BuildingState.Completed) building.FinishConstruction();
                        else if (state == BuildingState.UnderConstruction) building.BeginConstruction();
                    }
                }
            }

            // 3. Restore Construction Jobs
            if (dto.ConstructionData != null && cm != null)
            {
                foreach (var jobDTO in dto.ConstructionData.ActiveJobs)
                {
                    if (buildingLookup.TryGetValue(jobDTO.BuildingID, out var b))
                    {
                        cm.RestoreJob(b, jobDTO.TotalTime, jobDTO.TimeRemaining, jobDTO.IsPaused);
                    }
                }
            }
        }
    }
}
