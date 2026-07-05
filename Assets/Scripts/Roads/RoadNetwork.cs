using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.Roads
{
    /// <summary>
    /// The mathematical container for all roads in the city.
    /// Completely separated from visual rendering.
    /// </summary>
    public class RoadNetwork
    {
        public Dictionary<string, RoadNode> Nodes { get; private set; } = new Dictionary<string, RoadNode>();
        public Dictionary<string, RoadSegment> Segments { get; private set; } = new Dictionary<string, RoadSegment>();

        public RoadNode AddNode(Vector3 position)
        {
            // Check for existing node very close to this position to snap
            foreach (var existingNode in Nodes.Values)
            {
                if (Vector3.Distance(existingNode.Position, position) < 0.1f)
                {
                    return existingNode;
                }
            }

            RoadNode node = new RoadNode(position);
            Nodes.Add(node.ID, node);
            return node;
        }

        public RoadSegment AddSegment(RoadNode start, RoadNode end, Core.Configs.Roads.RoadSettings settings, List<Vector3> curvePoints = null)
        {
            RoadSegment segment = new RoadSegment(start, end, settings);
            if (curvePoints != null)
            {
                segment.SplinePoints.AddRange(curvePoints);
            }
            segment.CalculateLength();
            Segments.Add(segment.ID, segment);
            return segment;
        }

        public void RemoveSegment(string id)
        {
            if (Segments.TryGetValue(id, out RoadSegment segment))
            {
                segment.DestroySegment();
                Segments.Remove(id);
                
                // Cleanup orphaned nodes
                if (segment.StartNode.ConnectedSegments.Count == 0) Nodes.Remove(segment.StartNode.ID);
                if (segment.EndNode.ConnectedSegments.Count == 0) Nodes.Remove(segment.EndNode.ID);
            }
        }

        public void Clear()
        {
            Nodes.Clear();
            Segments.Clear();
        }
    }
}
