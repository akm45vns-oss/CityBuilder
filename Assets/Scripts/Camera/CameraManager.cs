using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Core.Configs;
using CityBuilder.Core.Logging;
using System.Collections.Generic;

namespace CityBuilder.Camera
{
    /// <summary>
    /// Manages the RTS Camera Rig and State Machine.
    /// Requires a specific Rig hierarchy: Root -> Pivot -> MainCamera
    /// </summary>
    public class CameraManager : Singleton<CameraManager>, IService
    {
        [Header("References")]
        public Transform CameraRoot;
        public Transform CameraPivot;
        public UnityEngine.Camera MainCamera;
        public CameraSettings Settings;

        private ICameraState _currentState;
        private Dictionary<System.Type, ICameraState> _states = new Dictionary<System.Type, ICameraState>();
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized) return;

            if (CameraRoot == null || CameraPivot == null || MainCamera == null || Settings == null)
            {
                GameLogger.Warning("[CameraManager] Missing references. Camera will not function properly.");
                return;
            }

            ServiceLocator.Register<CameraManager>(this);

            // Initialize States
            _states.Add(typeof(FreeCameraState), new FreeCameraState(this));
            _states.Add(typeof(FocusCameraState), new FocusCameraState(this));
            _states.Add(typeof(CinematicCameraState), new CinematicCameraState(this));

            SwitchState<FreeCameraState>();

            GameLogger.Info("[CameraManager] Initialized.");
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            _currentState?.UpdateState();
        }

        public void SwitchState<T>() where T : ICameraState
        {
            var type = typeof(T);
            if (_states.TryGetValue(type, out var newState))
            {
                _currentState?.Exit();
                _currentState = newState;
                _currentState.Enter();
                GameLogger.Verbose($"[CameraManager] Switched to {type.Name}");
            }
        }

        public ICameraState GetCurrentState() => _currentState;

        public void ResetCamera()
        {
            CameraRoot.position = Vector3.zero;
            CameraRoot.rotation = Quaternion.identity;
            CameraPivot.localRotation = Quaternion.Euler(45f, 0, 0);
            MainCamera.transform.localPosition = new Vector3(0, 0, -20f);
        }
    }
}
