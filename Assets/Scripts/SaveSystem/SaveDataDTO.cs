using System;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.SaveSystem
{
    [Serializable]
    public class SaveDataDTO
    {
        public int SaveVersion;
        public string Timestamp;
        public string Checksum;

        // Subsystem DTOs
        public TerrainSaveDTO TerrainData;
        public RoadNetworkSaveDTO RoadNetworkData;
        public BuildingSaveDTO BuildingData;
        public ConstructionSaveDTO ConstructionData;
    }

    [Serializable]
    public class TerrainSaveDTO
    {
        public int Seed;
        public int Resolution;
        public float[] Heightmap;
    }

    [Serializable]
    public class RoadNodeDTO
    {
        public string ID;
        public Vector3 Position;
    }

    [Serializable]
    public class RoadSegmentDTO
    {
        public string ID;
        public string StartNodeID;
        public string EndNodeID;
        public string SettingsName;
        public List<Vector3> SplinePoints;
    }

    [Serializable]
    public class RoadNetworkSaveDTO
    {
        public List<RoadNodeDTO> Nodes = new List<RoadNodeDTO>();
        public List<RoadSegmentDTO> Segments = new List<RoadSegmentDTO>();
    }

    [Serializable]
    public class BuildingInstanceDTO
    {
        public string ID;
        public string DefinitionID;
        public Vector3 Position;
        public int RotationSteps;
        public string State;
        
        // Road connection metadata
        public string ConnectedRoadSegmentID;
        public Vector3 EntrancePosition;

        // Sim data
        public int CurrentWorkers;
        public int CurrentResidents;
        public int CurrentUpgradeLevel;
    }

    [Serializable]
    public class BuildingSaveDTO
    {
        public List<BuildingInstanceDTO> Instances = new List<BuildingInstanceDTO>();
    }

    [Serializable]
    public class ConstructionJobDTO
    {
        public string BuildingID;
        public float TotalTime;
        public float TimeRemaining;
        public bool IsPaused;
    }

    [Serializable]
    public class ConstructionSaveDTO
    {
        public List<ConstructionJobDTO> ActiveJobs = new List<ConstructionJobDTO>();
    }
}
