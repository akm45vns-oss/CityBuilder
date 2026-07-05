using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "CityBuilder/Configs/GameplaySettings")]
    public class GameplaySettings : ScriptableObject
    {
        [Header("Gameplay")]
        public float DefaultCameraPanSpeed = 10f;
        public float DefaultCameraZoomSpeed = 5f;
        public int StartingFunds = 100000;
    }
}
