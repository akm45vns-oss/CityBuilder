using UnityEngine;

namespace CityBuilder.Terrain
{
    /// <summary>
    /// Command pattern implementation for Undo/Redo of heightmap changes.
    /// </summary>
    public class HeightmapModifyCommand : ITerrainCommand
    {
        private UnityEngine.Terrain _terrain;
        private int _startX, _startZ;
        private float[,] _oldHeights;
        private float[,] _newHeights;

        public HeightmapModifyCommand(UnityEngine.Terrain terrain, int startX, int startZ, float[,] oldHeights, float[,] newHeights)
        {
            _terrain = terrain;
            _startX = startX;
            _startZ = startZ;
            _oldHeights = oldHeights;
            _newHeights = newHeights;
        }

        public void Execute()
        {
            _terrain.terrainData.SetHeights(_startX, _startZ, _newHeights);
        }

        public void Undo()
        {
            _terrain.terrainData.SetHeights(_startX, _startZ, _oldHeights);
        }
    }
}
