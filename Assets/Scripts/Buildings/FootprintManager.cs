using UnityEngine;
using System.Collections.Generic;
using CityBuilder.Core.Configs.Buildings;
using CityBuilder.Grid;

namespace CityBuilder.Buildings
{
    /// <summary>
    /// Resolves building footprint offsets into actual GridCell references.
    /// Handles rotation of irregular footprints.
    /// </summary>
    public static class FootprintManager
    {
        /// <summary>
        /// Rotates a local footprint offset by 90-degree increments.
        /// </summary>
        public static Vector2Int RotateOffset(Vector2Int offset, int rotationSteps)
        {
            rotationSteps = ((rotationSteps % 4) + 4) % 4;
            Vector2Int result = offset;
            for (int i = 0; i < rotationSteps; i++)
            {
                result = new Vector2Int(result.y, -result.x);
            }
            return result;
        }

        /// <summary>
        /// Resolves all footprint cells for a given origin cell and rotation.
        /// Returns null if any cell is out of bounds.
        /// </summary>
        public static List<GridCell> ResolveCells(
            GridSystem grid,
            int originX,
            int originZ,
            BuildingDefinition definition,
            int rotationSteps)
        {
            List<Vector2Int> offsets = definition.GetFootprintOffsets();
            List<GridCell> cells = new List<GridCell>(offsets.Count);

            // Calculate offset pivot to center the footprint on the cursor
            Vector2Int pivot = Vector2Int.zero;
            if (definition.CustomFootprint == null || definition.CustomFootprint.Count == 0)
            {
                pivot = new Vector2Int(definition.Size.x / 2, definition.Size.y / 2);
            }

            foreach (var rawOffset in offsets)
            {
                Vector2Int rotated = RotateOffset(rawOffset - pivot, rotationSteps);
                int cellX = originX + rotated.x;
                int cellZ = originZ + rotated.y;

                GridCell cell = grid.GetCell(cellX, cellZ);
                if (cell == null) return null; // Out of bounds
                cells.Add(cell);
            }

            return cells;
        }
    }
}
