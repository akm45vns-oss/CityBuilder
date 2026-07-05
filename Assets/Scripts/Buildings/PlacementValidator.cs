using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Grid;
using CityBuilder.Roads;
using System.Collections.Generic;

namespace CityBuilder.Buildings
{
    public enum PlacementError
    {
        None,
        OutOfBounds,
        Occupied,
        SlopeTooSteep,
        NoRoadAccess,
        Underwater,
        InvalidTerrain
    }

    /// <summary>
    /// Validates whether a building can be placed at a given cell.
    /// Also queries the road network to find the nearest road and entrance position.
    /// </summary>
    public static class PlacementValidator
    {
        public static PlacementError Validate(
            GridSystem grid,
            int originX,
            int originZ,
            BuildingDefinition definition,
            int rotationSteps,
            out List<GridCell> targetCells,
            out RoadSegment connectedRoad,
            out Vector3 entrancePosition)
        {
            targetCells = null;
            connectedRoad = null;
            entrancePosition = Vector3.zero;

            var tm = ServiceLocator.Get<Managers.TerrainManager>();
            if (tm == null || grid == null) return PlacementError.OutOfBounds;

            // Resolve full footprint via FootprintManager
            List<GridCell> cells = FootprintManager.ResolveCells(grid, originX, originZ, definition, rotationSteps);
            if (cells == null) return PlacementError.OutOfBounds;

            foreach (var cell in cells)
            {
                if (cell.IsOccupied) return PlacementError.Occupied;
                if (cell.WorldPosition.y < tm.GridSettings.WaterLevel) return PlacementError.Underwater;
                if (!cell.IsBuildable) return PlacementError.SlopeTooSteep;
            }

            targetCells = cells;

            // Road Access Check
            if (definition.RequiresRoadAccess)
            {
                FindNearestRoad(cells, out connectedRoad, out entrancePosition);
                if (connectedRoad == null) return PlacementError.NoRoadAccess;
            }

            return PlacementError.None;
        }

        private static void FindNearestRoad(
            List<GridCell> footprintCells,
            out RoadSegment nearestSegment,
            out Vector3 entrancePos)
        {
            nearestSegment = null;
            entrancePos = Vector3.zero;

            var rm = ServiceLocator.Get<Managers.RoadManager>();
            if (rm == null || rm.Network == null || rm.Network.Segments.Count == 0) return;

            float bestDist = float.MaxValue;

            // Use the centroid of the footprint to query nearest road
            Vector3 centroid = Vector3.zero;
            foreach (var cell in footprintCells) centroid += cell.WorldPosition;
            centroid /= footprintCells.Count;

            foreach (var segment in rm.Network.Segments.Values)
            {
                // Find closest point on segment line
                Vector3 start = segment.StartNode.Position;
                Vector3 end = segment.EndNode.Position;
                Vector3 closest = ClosestPointOnSegment(centroid, start, end);
                float dist = Vector3.Distance(centroid, closest);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    nearestSegment = segment;
                    entrancePos = closest;
                }
            }
        }

        private static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector3 ab = b - a;
            float t = Vector3.Dot(point - a, ab) / Vector3.Dot(ab, ab);
            t = Mathf.Clamp01(t);
            return a + t * ab;
        }
    }
}
