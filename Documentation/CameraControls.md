# Camera Controls

The camera controls are completely driven by the `InputManager` parsing the Unity New Input System and passed to the `FreeCameraState`.

## Bindings
| Action | Binding | Description |
|--------|---------|-------------|
| **Pan** | `W/A/S/D` or `Arrows` | Moves the camera across the X/Z plane. |
| **Fast Pan** | `Shift` (Hold) + Move | Multiplies movement speed by `FastMoveSpeed`. |
| **Edge Scroll** | Mouse to Screen Edge | Pans the camera. Can be toggled on/off. |
| **Mouse Pan** | `Middle Mouse Button` (Hold) | Drags the camera across the X/Z plane. |
| **Rotate (Yaw)** | `Q` / `E` | Rotates the camera left and right around its root. |
| **Mouse Rotate** | `Ctrl` + `Middle Mouse Button` | Moves mouse X for Yaw, Mouse Y for Pitch (Tilt). |
| **Zoom** | `Scroll Wheel` | Adjusts the local Z distance of the main camera from the pivot. |
| **Reset** | `R` | Resets the camera to `(0,0,0)` with default zoom and tilt. |
| **Toggle Edge Scroll** | `T` | Enables or disables edge scrolling. |

*Note: Speeds, inverted axes, and tilt limits are fully configurable via the `CameraSettings` ScriptableObject.*
