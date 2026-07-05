using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "GraphicsSettings", menuName = "CityBuilder/Configs/GraphicsSettings")]
    public class GraphicsSettings : ScriptableObject
    {
        [Header("Graphics")]
        public int QualityLevel = 2; // e.g., 0=Low, 1=Medium, 2=High
        public FullScreenMode ScreenMode = FullScreenMode.ExclusiveFullScreen;
        public Vector2Int Resolution = new Vector2Int(1920, 1080);
    }
}
