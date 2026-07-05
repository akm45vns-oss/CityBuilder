using UnityEngine;
using CityBuilder.Core;
using CityBuilder.Managers;
using UnityEngine.InputSystem;

namespace CityBuilder.Camera
{
    public class FreeCameraState : ICameraState
    {
        private CameraManager _cam;
        private InputManager _input;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _targetZoom;
        private float _targetTilt;

        private bool _isEdgeScrollingEnabled;

        public FreeCameraState(CameraManager cam)
        {
            _cam = cam;
            _input = ServiceLocator.Get<InputManager>();
        }

        public void Enter()
        {
            _targetPosition = _cam.CameraRoot.position;
            _targetRotation = _cam.CameraRoot.rotation;
            _targetZoom = _cam.MainCamera.transform.localPosition.z;
            _targetTilt = _cam.CameraPivot.localEulerAngles.x;
            
            _isEdgeScrollingEnabled = _cam.Settings.EdgeScrollEnabled;

            if (_input != null)
            {
                _input.ResetCameraAction.performed += OnResetCamera;
                _input.ToggleEdgeScrollAction.performed += OnToggleEdgeScroll;
            }
        }

        public void Exit()
        {
            if (_input != null)
            {
                _input.ResetCameraAction.performed -= OnResetCamera;
                _input.ToggleEdgeScrollAction.performed -= OnToggleEdgeScroll;
            }
        }

        public void UpdateState()
        {
            if (_input == null) return;

            HandleMovement();
            HandleRotation();
            HandleZoom();

            ApplySmoothing();
        }

        private void HandleMovement()
        {
            Vector2 moveInput = _input.MoveAction.ReadValue<Vector2>();
            Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

            // Edge Scrolling
            if (_isEdgeScrollingEnabled && moveInput == Vector2.zero)
            {
                Vector2 mousePos = _input.GetMousePosition();
                if (mousePos.x < _cam.Settings.EdgeScrollSize) moveDir.x = -1f;
                if (mousePos.x > Screen.width - _cam.Settings.EdgeScrollSize) moveDir.x = 1f;
                if (mousePos.y < _cam.Settings.EdgeScrollSize) moveDir.z = -1f;
                if (mousePos.y > Screen.height - _cam.Settings.EdgeScrollSize) moveDir.z = 1f;
            }

            // Middle Mouse Pan
            if (_input.PanAction.IsPressed())
            {
                Vector2 delta = _input.GetMouseDelta();
                moveDir.x = -delta.x * 0.1f;
                moveDir.z = -delta.y * 0.1f;
            }

            // Calculate Speed
            float speed = Keyboard.current.shiftKey.isPressed ? _cam.Settings.FastMoveSpeed : _cam.Settings.MoveSpeed;

            // Apply orientation
            Vector3 worldMove = _cam.CameraRoot.TransformDirection(moveDir) * speed * Time.deltaTime;
            _targetPosition += worldMove;

            // Clamp Position to Map Bounds
            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _cam.Settings.MapBoundsMin.x, _cam.Settings.MapBoundsMax.x);
            _targetPosition.z = Mathf.Clamp(_targetPosition.z, _cam.Settings.MapBoundsMin.y, _cam.Settings.MapBoundsMax.y);
            
            // Simple terrain collision (prevent going underground)
            float terrainHeight = 0f; // Future: Terrain.activeTerrain.SampleHeight(_targetPosition);
            _targetPosition.y = terrainHeight; 
        }

        private void HandleRotation()
        {
            // Keyboard Rotation (Q/E)
            float rotInput = _input.RotateAction.ReadValue<float>();
            float rotDir = _cam.Settings.InvertRotation ? -1f : 1f;

            _targetRotation *= Quaternion.Euler(Vector3.up * rotInput * _cam.Settings.RotationSpeed * rotDir * Time.deltaTime);

            // Middle Mouse Rotation
            if (_input.PanAction.IsPressed() && Keyboard.current.ctrlKey.isPressed)
            {
                Vector2 delta = _input.GetMouseDelta();
                _targetRotation *= Quaternion.Euler(Vector3.up * delta.x * _cam.Settings.MiddleMouseRotationSpeed * Time.deltaTime);
                
                // Tilt (Pitch)
                _targetTilt += delta.y * _cam.Settings.MiddleMouseRotationSpeed * Time.deltaTime;
                _targetTilt = Mathf.Clamp(_targetTilt, _cam.Settings.MinTiltAngle, _cam.Settings.MaxTiltAngle);
            }
        }

        private void HandleZoom()
        {
            float zoomInput = _input.ZoomAction.ReadValue<float>();
            if (Mathf.Abs(zoomInput) > 0.1f)
            {
                float zoomDir = _cam.Settings.InvertZoom ? -1f : 1f;
                _targetZoom += Mathf.Sign(zoomInput) * _cam.Settings.ZoomSpeed * zoomDir;
                
                // Negative Z is standard for camera distance
                _targetZoom = Mathf.Clamp(_targetZoom, -_cam.Settings.MaxHeight, -_cam.Settings.MinHeight);
            }
        }

        private void ApplySmoothing()
        {
            float smoothSpeed = _cam.Settings.SmoothTime * Time.deltaTime;

            _cam.CameraRoot.position = Vector3.Lerp(_cam.CameraRoot.position, _targetPosition, smoothSpeed);
            _cam.CameraRoot.rotation = Quaternion.Lerp(_cam.CameraRoot.rotation, _targetRotation, smoothSpeed);
            
            _cam.CameraPivot.localRotation = Quaternion.Lerp(_cam.CameraPivot.localRotation, Quaternion.Euler(_targetTilt, 0, 0), smoothSpeed);
            
            Vector3 camLocalPos = _cam.MainCamera.transform.localPosition;
            camLocalPos.z = Mathf.Lerp(camLocalPos.z, _targetZoom, smoothSpeed);
            _cam.MainCamera.transform.localPosition = camLocalPos;
        }

        private void OnResetCamera(InputAction.CallbackContext ctx)
        {
            _cam.ResetCamera();
            Enter(); // Reset targets
        }

        private void OnToggleEdgeScroll(InputAction.CallbackContext ctx)
        {
            _isEdgeScrollingEnabled = !_isEdgeScrollingEnabled;
        }
    }
}
