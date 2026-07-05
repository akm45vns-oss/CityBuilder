using UnityEngine;
using UnityEngine.InputSystem;
using CityBuilder.Core;
using CityBuilder.Core.Logging;
using CityBuilder.Core.Selection;
using CityBuilder.Events;

namespace CityBuilder.Managers
{
    /// <summary>
    /// Handles Raycasting to select GameObjects in the scene that implement ISelectable.
    /// </summary>
    public class SelectionManager : Singleton<SelectionManager>, IService
    {
        private bool _isInitialized;
        private InputManager _input;
        
        public ISelectable CurrentSelection { get; private set; }

        public void Initialize()
        {
            if (_isInitialized) return;
            
            _input = ServiceLocator.Get<InputManager>();
            if (_input != null)
            {
                _input.SelectAction.performed += OnSelectPerformed;
            }
            
            ServiceLocator.Register<SelectionManager>(this);
            GameLogger.Info("[SelectionManager] Initialized.");
            _isInitialized = true;
        }

        private void OnSelectPerformed(InputAction.CallbackContext ctx)
        {
            // Only select if we're not clicking on UI (Implementation requires EventSystem checking)
            // if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            if (UnityEngine.Camera.main == null) return;

            Vector2 mousePos = _input.GetMousePosition();
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                ISelectable selectable = hit.collider.GetComponentInParent<ISelectable>();

                if (selectable != null)
                {
                    if (CurrentSelection != selectable)
                    {
                        DeselectCurrent();
                        CurrentSelection = selectable;
                        CurrentSelection.OnSelected();
                        EventBus.Broadcast(new ObjectSelectedEvent { SelectedObject = CurrentSelection });
                        GameLogger.Verbose($"[SelectionManager] Selected: {CurrentSelection.GetName()}");
                    }
                }
                else
                {
                    DeselectCurrent();
                }
            }
            else
            {
                DeselectCurrent();
            }
        }

        private void DeselectCurrent()
        {
            if (CurrentSelection != null)
            {
                EventBus.Broadcast(new ObjectDeselectedEvent { DeselectedObject = CurrentSelection });
                CurrentSelection.OnDeselected();
                CurrentSelection = null;
            }
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            if (_input != null)
            {
                _input.SelectAction.performed -= OnSelectPerformed;
            }
        }
    }
}
