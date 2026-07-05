using UnityEngine;
using CityBuilder.Core.Configs.Roads;
using System.Collections.Generic;
using CityBuilder.Grid;

using CityBuilder.Utilities;

namespace CityBuilder.Roads
{
    /// <summary>
    /// Represents an edge connecting two RoadNodes.
    /// Stores the settings (speed limit, lanes) and calculated length.
    /// </summary>
    public class RoadSegment : ISpatialLocatable
    {
        public string ID { get; private set; }
        public Vector3 Position => (StartNode.Position + EndNode.Position) / 2f;
        public RoadNode StartNode { get; private set; }
        public RoadNode EndNode { get; private set; }
        public RoadSettings Settings { get; private set; }
        
        public float Length { get; private set; }
        
        // Points for spline curves. If empty, it's a straight line.
        public List<Vector3> SplinePoints { get; private set; } = new List<Vector3>();

        // Occupied grid cells underneath this segment
        public List<GridCell> OccupiedCells { get; private set; } = new List<GridCell>();

        public RoadSegment(RoadNode start, RoadNode end, RoadSettings settings)
        {
            ID = System.Guid.NewGuid().ToString();
            StartNode = start;
            EndNode = end;
            Settings = settings;

            CalculateLength();
            
            // Bidirectional graph connection
            StartNode.AddConnection(this);
            EndNode.AddConnection(this);
        }

        public void CalculateLength()
        {
            if (SplinePoints.Count > 0)
            {
                Length = 0f;
                for (int i = 0; i < SplinePoints.Count - 1; i++)
                {
                    Length += Vector3.Distance(SplinePoints[i], SplinePoints[i + 1]);
                }
            }
            else
            {
                Length = Vector3.Distance(StartNode.Position, EndNode.Position);
            }
        }
        
        public void DestroySegment()
        {
            StartNode.RemoveConnection(this);
            EndNode.RemoveConnection(this);
            
            // Free grid cells
            foreach (var cell in OccupiedCells)
            {
                cell.Free();
            }
        }
    }
}
