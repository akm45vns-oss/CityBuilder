using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "CityBuilder/Configs/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [Header("Audio Volumes")]
        [Range(0f, 1f)] public float MasterVolume = 1.0f;
        [Range(0f, 1f)] public float MusicVolume = 0.8f;
        [Range(0f, 1f)] public float SFXVolume = 1.0f;
    }
}
