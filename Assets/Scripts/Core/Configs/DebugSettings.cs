using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "CityBuilder/Configs/DebugSettings")]
    public class DebugSettings : ScriptableObject
    {
        [Header("Debug")]
        public bool ShowFPSCounter = true;
        public bool ShowMemoryUsage = true;
        public bool EnableDebugOverlay = false;
        public bool LogVerboseMessages = false;
    }
}
