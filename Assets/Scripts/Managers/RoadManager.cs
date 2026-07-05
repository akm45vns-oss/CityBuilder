using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Roads;
using CityBuilder.Core.Configs.Roads;

namespace CityBuilder.Managers
{
    /// <summary>
    /// The IService entry point for all road operations.
    /// </summary>
    public class RoadManager : Singleton<RoadManager>, IService
    {
        private bool _isInitialized;
        
        public RoadNetwork Network { get; private set; }
        
        [Header("References")]
        public RoadMeshGenerator MeshGenerator;

        public CityBuilder.Utilities.SpatialHashGrid<RoadSegment> SegmentSpatialGrid { get; private set; }
        public CityBuilder.Utilities.SpatialHashGrid<RoadNode> NodeSpatialGrid { get; private set; }

        public void Initialize()
        {
            if (_isInitialized) return;

            Network = new RoadNetwork();
            SegmentSpatialGrid = new CityBuilder.Utilities.SpatialHashGrid<RoadSegment>(32f);
            NodeSpatialGrid = new CityBuilder.Utilities.SpatialHashGrid<RoadNode>(32f);
            
            if (MeshGenerator == null)
            {
                // Auto-create mesh generator if missing
                GameObject genObj = new GameObject("RoadMeshGenerator");
                genObj.transform.SetParent(transform);
                MeshGenerator = genObj.AddComponent<RoadMeshGenerator>();
            }

            ServiceLocator.Register<RoadManager>(this);
            GameLogger.Info("[RoadManager] Initialized with empty Road Network.");
            _isInitialized = true;
        }

        public void BuildRoad(Vector3 startPoint, Vector3 endPoint, RoadSettings settings)
        {
            // Update Graph
            RoadNode startNode = Network.AddNode(startPoint);
            RoadNode endNode = Network.AddNode(endPoint);
            RoadSegment segment = Network.AddSegment(startNode, endNode, settings);

            // Register in Spatial Hash
            NodeSpatialGrid.Add(startNode);
            NodeSpatialGrid.Add(endNode);
            SegmentSpatialGrid.Add(segment);

            // Calculate Grid Occupancy
            MarkGridCells(segment);

            // Update Visuals
            MeshGenerator.RebuildMesh(segment);
            MeshGenerator.RebuildIntersection(startNode);
            MeshGenerator.RebuildIntersection(endNode);
            
            GameLogger.Verbose($"[RoadManager] Built road segment: {segment.ID}");
        }
        
        public void RemoveRoad(RoadSegment segment)
        {
            RoadNode n1 = segment.StartNode;
            RoadNode n2 = segment.EndNode;
            
            SegmentSpatialGrid.Remove(segment);
            Network.RemoveSegment(segment.ID);
            MeshGenerator.DestroyMesh(segment.ID);
            
            // Rebuild adjacent intersections and update spatial node tags
            if (Network.Nodes.ContainsKey(n1.ID))
            {
                MeshGenerator.RebuildIntersection(n1);
            }
            else
            {
                MeshGenerator.DestroyIntersection(n1.ID);
                NodeSpatialGrid.Remove(n1);
            }
            
            if (Network.Nodes.ContainsKey(n2.ID))
            {
                MeshGenerator.RebuildIntersection(n2);
            }
            else
            {
                MeshGenerator.DestroyIntersection(n2.ID);
                NodeSpatialGrid.Remove(n2);
            }
        }

        private void MarkGridCells(RoadSegment segment)
        {
            var tm = ServiceLocator.Get<TerrainManager>();
            if (tm == null || tm.Grid == null) return;
            
            // Basic line rasterization over grid cells
            // In full implementation, uses Bresenham's line algorithm mapped to cell size
            var startCell = tm.Grid.GetCellFromWorldPosition(segment.StartNode.Position, tm.ActiveTerrain);
            var endCell = tm.Grid.GetCellFromWorldPosition(segment.EndNode.Position, tm.ActiveTerrain);
            
            if (startCell != null)
            {
                startCell.ReservationState = CellReservationState.Occupied;
                segment.OccupiedCells.Add(startCell);
            }
            if (endCell != null && endCell != startCell)
            {
                endCell.ReservationState = CellReservationState.Occupied;
                segment.OccupiedCells.Add(endCell);
            }
        }
    }
}
