using UnityEngine;
using System.Collections.Generic;

using CityBuilder.Utilities;

namespace CityBuilder.Roads
{
    /// <summary>
    /// Represents an intersection, endpoint, or curve control point in the road graph.
    /// The source of truth for connectivity.
    /// </summary>
    public class RoadNode : ISpatialLocatable
    {
        public string ID { get; private set; }
        public Vector3 Position { get; set; }
        
        // Connections to other nodes via segments
        public List<RoadSegment> ConnectedSegments { get; private set; } = new List<RoadSegment>();

        public RoadNode(Vector3 position)
        {
            ID = System.Guid.NewGuid().ToString();
            Position = position;
        }

        public void AddConnection(RoadSegment segment)
        {
            if (!ConnectedSegments.Contains(segment))
            {
                ConnectedSegments.Add(segment);
            }
        }

        public void RemoveConnection(RoadSegment segment)
        {
            ConnectedSegments.Remove(segment);
        }
        
        public bool IsDeadEnd() => ConnectedSegments.Count == 1;
        public bool IsStraightOrCorner() => ConnectedSegments.Count == 2;
        public bool IsTJunction() => ConnectedSegments.Count == 3;
        public bool IsCrossJunction() => ConnectedSegments.Count == 4;
    }
}
