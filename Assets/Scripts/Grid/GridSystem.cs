using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Configs.Terrain;
using CityBuilder.Core.Logging;

namespace CityBuilder.Grid
{
    /// <summary>
    /// Generates and manages the 2D array of GridCells over the terrain.
    /// </summary>
    public class GridSystem
    {
        public GridCell[,] Cells { get; private set; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        private float _cellSize;

        public void GenerateGrid(UnityEngine.Terrain terrain, GridSettings settings)
        {
            if (terrain == null || settings == null)
            {
                GameLogger.Error("[GridSystem] Missing Terrain or GridSettings.");
                return;
            }

            _cellSize = settings.CellSize;
            Vector3 terrainSize = terrain.terrainData.size;
            Vector3 terrainPos = terrain.transform.position;

            Width = Mathf.FloorToInt(terrainSize.x / _cellSize);
            Height = Mathf.FloorToInt(terrainSize.z / _cellSize);

            Cells = new GridCell[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    Vector3 worldPos = new Vector3(
                        terrainPos.x + (x * _cellSize) + (_cellSize / 2f),
                        0,
                        terrainPos.z + (z * _cellSize) + (_cellSize / 2f)
                    );

                    float y = terrain.SampleHeight(worldPos);
                    worldPos.y = y;

                    // Calculate Slope by sampling normals
                    float normalizedX = (worldPos.x - terrainPos.x) / terrainSize.x;
                    float normalizedZ = (worldPos.z - terrainPos.z) / terrainSize.z;
                    Vector3 normal = terrain.terrainData.GetInterpolatedNormal(normalizedX, normalizedZ);
                    float slope = Vector3.Angle(Vector3.up, normal);

                    GridCell cell = new GridCell(x, z, worldPos, y, slope);
                    
                    // Determine buildability based on slope and water level
                    cell.IsBuildable = (slope <= settings.MaxBuildableSlope) && (y >= settings.WaterLevel);
                    cell.IsWalkable = cell.IsBuildable; // Simplified for now

                    Cells[x, z] = cell;
                }
            }

            GameLogger.Info($"[GridSystem] Generated {Width}x{Height} grid foundation.");
        }

        public GridCell GetCell(int x, int z)
        {
            if (x >= 0 && x < Width && z >= 0 && z < Height)
                return Cells[x, z];
            return null;
        }

        public GridCell GetCellFromWorldPosition(Vector3 worldPosition, UnityEngine.Terrain terrain)
        {
            Vector3 localPos = worldPosition - terrain.transform.position;
            int x = Mathf.FloorToInt(localPos.x / _cellSize);
            int z = Mathf.FloorToInt(localPos.z / _cellSize);
            return GetCell(x, z);
        }
    }
}
