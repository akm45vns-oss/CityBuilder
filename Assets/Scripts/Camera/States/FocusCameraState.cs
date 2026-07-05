using UnityEngine;

namespace CityBuilder.Camera
{
    public class FocusCameraState : ICameraState
    {
        private CameraManager _cam;
        private Transform _target;

        public FocusCameraState(CameraManager cam)
        {
            _cam = cam;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Enter()
        {
            // Initialization
        }

        public void Exit()
        {
            _target = null;
        }

        public void UpdateState()
        {
            if (_target == null)
            {
                _cam.SwitchState<FreeCameraState>();
                return;
            }

            float smoothSpeed = _cam.Settings.SmoothTime * Time.deltaTime;
            _cam.CameraRoot.position = Vector3.Lerp(_cam.CameraRoot.position, _target.position, smoothSpeed);

            // Allow user to break focus
            if (UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame)
            {
                _cam.SwitchState<FreeCameraState>();
            }
        }
    }
}
