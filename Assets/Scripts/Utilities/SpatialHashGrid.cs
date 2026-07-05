using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace CityBuilder.Utilities
{
    /// <summary>
    /// Thread-safe 2D Spatial Hash Grid to index objects in world coordinates.
    /// Exposes extremely fast O(1) radius and nearest-neighbor lookups.
    /// </summary>
    public class SpatialHashGrid<T> where T : class, ISpatialLocatable
    {
        private readonly float _cellSize;
        private readonly Dictionary<Vector2Int, List<T>> _grid = new Dictionary<Vector2Int, List<T>>();
        private readonly Dictionary<string, Vector2Int> _positions = new Dictionary<string, Vector2Int>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        public SpatialHashGrid(float cellSize = 32f)
        {
            _cellSize = cellSize;
        }

        private Vector2Int GetKey(Vector3 position)
        {
            return new Vector2Int(
                Mathf.FloorToInt(position.x / _cellSize),
                Mathf.FloorToInt(position.z / _cellSize)
            );
        }

        public void Add(T item)
        {
            if (item == null) return;
            Vector2Int key = GetKey(item.Position);

            _lock.EnterWriteLock();
            try
            {
                if (!_grid.TryGetValue(key, out var cellList))
                {
                    cellList = new List<T>();
                    _grid[key] = cellList;
                }
                cellList.Add(item);
                _positions[item.ID] = key;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Remove(T item)
        {
            if (item == null) return;

            _lock.EnterWriteLock();
            try
            {
                if (_positions.TryGetValue(item.ID, out Vector2Int key))
                {
                    if (_grid.TryGetValue(key, out var cellList))
                    {
                        cellList.Remove(item);
                        if (cellList.Count == 0)
                        {
                            _grid.Remove(key);
                        }
                    }
                    _positions.Remove(item.ID);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Update(T item)
        {
            if (item == null) return;
            Vector2Int newKey = GetKey(item.Position);

            _lock.EnterWriteLock();
            try
            {
                if (_positions.TryGetValue(item.ID, out Vector2Int oldKey))
                {
                    if (oldKey == newKey) return; // Unchanged

                    // Remove from old
                    if (_grid.TryGetValue(oldKey, out var oldList))
                    {
                        oldList.Remove(item);
                        if (oldList.Count == 0) _grid.Remove(oldKey);
                    }
                }

                // Add to new
                if (!_grid.TryGetValue(newKey, out var newList))
                {
                    newList = new List<T>();
                    _grid[newKey] = newList;
                }
                newList.Add(item);
                _positions[item.ID] = newKey;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void QueryRadius(Vector3 center, float radius, List<T> results)
        {
            results.Clear();
            Vector2Int minKey = GetKey(center - new Vector3(radius, 0f, radius));
            Vector2Int maxKey = GetKey(center + new Vector3(radius, 0f, radius));
            float sqrRadius = radius * radius;

            _lock.EnterReadLock();
            try
            {
                for (int x = minKey.x; x <= maxKey.x; x++)
                {
                    for (int y = minKey.y; y <= maxKey.y; y++)
                    {
                        Vector2Int key = new Vector2Int(x, y);
                        if (_grid.TryGetValue(key, out var cellList))
                        {
                            for (int i = 0; i < cellList.Count; i++)
                            {
                                T item = cellList[i];
                                float distSqr = (item.Position - center).sqrMagnitude;
                                if (distSqr <= sqrRadius)
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public T QueryNearest(Vector3 center, float maxRadius = float.MaxValue)
        {
            T nearest = null;
            float nearestDistSqr = maxRadius * maxRadius;

            Vector2Int minKey = GetKey(center - new Vector3(maxRadius == float.MaxValue ? 1000f : maxRadius, 0f, maxRadius == float.MaxValue ? 1000f : maxRadius));
            Vector2Int maxKey = GetKey(center + new Vector3(maxRadius == float.MaxValue ? 1000f : maxRadius, 0f, maxRadius == float.MaxValue ? 1000f : maxRadius));

            _lock.EnterReadLock();
            try
            {
                for (int x = minKey.x; x <= maxKey.x; x++)
                {
                    for (int y = minKey.y; y <= maxKey.y; y++)
                    {
                        Vector2Int key = new Vector2Int(x, y);
                        if (_grid.TryGetValue(key, out var cellList))
                        {
                            for (int i = 0; i < cellList.Count; i++)
                            {
                                T item = cellList[i];
                                float distSqr = (item.Position - center).sqrMagnitude;
                                if (distSqr < nearestDistSqr)
                                {
                                    nearestDistSqr = distSqr;
                                    nearest = item;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return nearest;
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _grid.Clear();
                _positions.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
