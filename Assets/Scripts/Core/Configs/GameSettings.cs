using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "CityBuilder/Configs/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Application")]
        public int TargetFrameRate = 60;
        public bool VSync = true;
        public string Language = "en";
        public bool AutosaveEnabled = true;
        public int AutosaveIntervalMinutes = 10;
    }
}
