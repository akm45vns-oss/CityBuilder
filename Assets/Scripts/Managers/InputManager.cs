using UnityEngine;
using UnityEngine.InputSystem;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using System;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Centralized input handling using the New Input System.
    /// Programmatically generates actions to avoid missing .inputactions asset issues.
    /// </summary>
    public class InputManager : Singleton<InputManager>, IService
    {
        private bool _isInitialized;

        public InputActionMap PlayerMap { get; private set; }

        // Actions
        public InputAction MoveAction { get; private set; }
        public InputAction RotateAction { get; private set; }
        public InputAction ZoomAction { get; private set; }
        public InputAction PanAction { get; private set; }
        public InputAction FocusAction { get; private set; }
        public InputAction ResetCameraAction { get; private set; }
        public InputAction ToggleEdgeScrollAction { get; private set; }
        public InputAction SelectAction { get; private set; }

        public void Initialize()
        {
            if (_isInitialized) return;

            CreateInputActions();
            
            ServiceLocator.Register<InputManager>(this);
            GameLogger.Info("[InputManager] Initialized with New Input System.");
            _isInitialized = true;
        }

        private void CreateInputActions()
        {
            PlayerMap = new InputActionMap("Player");

            // WASD or Arrows
            MoveAction = PlayerMap.AddAction("Move", type: InputActionType.Value);
            MoveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");

            // Q/E for Rotation
            RotateAction = PlayerMap.AddAction("Rotate", type: InputActionType.Value);
            RotateAction.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/q")
                .With("Positive", "<Keyboard>/e");

            // Scroll Wheel
            ZoomAction = PlayerMap.AddAction("Zoom", type: InputActionType.Value, binding: "<Mouse>/scroll/y");

            // Middle Mouse Drag
            PanAction = PlayerMap.AddAction("Pan", type: InputActionType.Button, binding: "<Mouse>/middleButton");

            // Focus and Reset
            FocusAction = PlayerMap.AddAction("Focus", type: InputActionType.Button, binding: "<Keyboard>/f");
            ResetCameraAction = PlayerMap.AddAction("ResetCamera", type: InputActionType.Button, binding: "<Keyboard>/r");

            // Edge Scroll Toggle
            ToggleEdgeScrollAction = PlayerMap.AddAction("ToggleEdgeScroll", type: InputActionType.Button, binding: "<Keyboard>/t");

            // Selection (Left Click)
            SelectAction = PlayerMap.AddAction("Select", type: InputActionType.Button, binding: "<Mouse>/leftButton");

            PlayerMap.Enable();
        }

        public Vector2 GetMousePosition()
        {
            return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        }

        public Vector2 GetMouseDelta()
        {
            return Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            PlayerMap?.Disable();
        }
    }
}
