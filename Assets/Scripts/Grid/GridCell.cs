using UnityEngine;

namespace CityBuilder.Grid
{
    public enum TerrainType
    {
        Grass,
        Dirt,
        Sand,
        Rock,
        Snow,
        Water
    }

    public enum CellReservationState
    {
        Free,
        Reserved,   // Claimed by a ghost (preview)
        Occupied    // Claimed by a permanent object
    }

    /// <summary>
    /// Represents a single cell in the world grid.
    /// Acts as the foundation for pathfinding, zoning, building, and utility logic.
    /// </summary>
    public class GridCell
    {
        public int X { get; private set; }
        public int Z { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public float Height { get; private set; }
        public float Slope { get; private set; }

        public bool IsWalkable { get; set; }
        public bool IsBuildable { get; set; }
        public bool IsOccupied => ReservationState == CellReservationState.Occupied;

        // Phase 4: Rich occupancy data
        public CellReservationState ReservationState { get; set; } = CellReservationState.Free;
        public string OccupyingBuildingID { get; set; }
        public float OccupyingBuildingRotation { get; set; }
        public bool HasRoadConnection { get; set; }

        public TerrainType GroundType { get; set; }

        // Future fields
        // public string RoadSegmentID { get; set; }

        public GridCell(int x, int z, Vector3 worldPos, float height, float slope)
        {
            X = x;
            Z = z;
            WorldPosition = worldPos;
            Height = height;
            Slope = slope;
            ReservationState = CellReservationState.Free;
        }

        public void Occupy(string buildingID, float rotation)
        {
            ReservationState = CellReservationState.Occupied;
            OccupyingBuildingID = buildingID;
            OccupyingBuildingRotation = rotation;
        }

        public void Free()
        {
            ReservationState = CellReservationState.Free;
            OccupyingBuildingID = null;
            OccupyingBuildingRotation = 0f;
            HasRoadConnection = false;
        }
    }
}
