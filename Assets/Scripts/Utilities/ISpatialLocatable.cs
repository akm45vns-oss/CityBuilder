using UnityEngine;

namespace CityBuilder.Utilities
{
    public interface ISpatialLocatable
    {
        string ID { get; }
        Vector3 Position { get; }
    }
}
