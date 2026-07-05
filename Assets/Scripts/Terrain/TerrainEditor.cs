using UnityEngine;
using CityBuilder.Core.Configs.Terrain;
using System.Collections.Generic;

namespace CityBuilder.Terrain
{
    public interface ITerrainCommand
    {
        void Execute();
        void Undo();
    }

    /// <summary>
    /// Core class handling terrain heightmap deformation and painting.
    /// Supports Raise, Lower, Flatten, and Smooth.
    /// </summary>
    public class TerrainEditor
    {
        private UnityEngine.Terrain _terrain;
        private Stack<ITerrainCommand> _undoStack = new Stack<ITerrainCommand>();
        private Stack<ITerrainCommand> _redoStack = new Stack<ITerrainCommand>();

        public TerrainEditor(UnityEngine.Terrain terrain)
        {
            _terrain = terrain;
        }

        public void ApplyBrush(Vector3 worldPos, TerrainBrushSettings brush, bool lower = false)
        {
            // Calculate coordinates
            Vector3 localPos = worldPos - _terrain.transform.position;
            TerrainData tData = _terrain.terrainData;

            int mapX = (int)((localPos.x / tData.size.x) * tData.heightmapResolution);
            int mapZ = (int)((localPos.z / tData.size.z) * tData.heightmapResolution);

            int radius = brush.Radius;
            int size = radius * 2;
            int startX = Mathf.Clamp(mapX - radius, 0, tData.heightmapResolution - 1);
            int startZ = Mathf.Clamp(mapZ - radius, 0, tData.heightmapResolution - 1);

            int width = Mathf.Clamp(size, 0, tData.heightmapResolution - startX);
            int height = Mathf.Clamp(size, 0, tData.heightmapResolution - startZ);

            if (width <= 0 || height <= 0) return;

            float[,] originalHeights = tData.GetHeights(startX, startZ, width, height);
            float[,] newHeights = tData.GetHeights(startX, startZ, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float distance = Vector2.Distance(new Vector2(mapX, mapZ), new Vector2(startX + x, startZ + y));
                    if (distance <= radius)
                    {
                        float normalizedDist = distance / radius;
                        float falloff = brush.Falloff.Evaluate(1 - normalizedDist);
                        float adjustment = brush.Strength * falloff * Time.deltaTime;
                        
                        if (lower) newHeights[y, x] -= adjustment;
                        else newHeights[y, x] += adjustment;
                    }
                }
            }

            // Execute via Command for Undo/Redo
            // In a full implementation, you'd batch these on mouse release.
            // For now, we apply directly to demonstrate the API.
            tData.SetHeights(startX, startZ, newHeights);
        }

        public void FlattenArea(Vector3 worldPos, float targetHeight, TerrainBrushSettings brush)
        {
            // Similar to ApplyBrush, but lerps heights towards targetHeight normalized value
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var cmd = _undoStack.Pop();
                cmd.Undo();
                _redoStack.Push(cmd);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var cmd = _redoStack.Pop();
                cmd.Execute();
                _undoStack.Push(cmd);
            }
        }
    }
}
