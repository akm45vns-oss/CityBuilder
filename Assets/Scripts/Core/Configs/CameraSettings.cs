using UnityEngine;

namespace CityBuilder.Core.Configs
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "CityBuilder/Configs/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Movement")]
        public float MoveSpeed = 20f;
        public float FastMoveSpeed = 50f;
        public float SmoothTime = 5f;

        [Header("Rotation & Tilt")]
        public float RotationSpeed = 100f;
        public float MiddleMouseRotationSpeed = 10f;
        public float MinTiltAngle = 30f;
        public float MaxTiltAngle = 80f;
        public bool InvertRotation = false;

        [Header("Zoom")]
        public float ZoomSpeed = 5f;
        public float MinHeight = 5f;
        public float MaxHeight = 100f;
        public bool InvertZoom = false;

        [Header("Edge Scrolling")]
        public bool EdgeScrollEnabled = true;
        public float EdgeScrollSize = 20f; // Pixels from edge

        [Header("Limits")]
        public Vector2 MapBoundsMin = new Vector2(-1000, -1000);
        public Vector2 MapBoundsMax = new Vector2(1000, 1000);
    }
}
