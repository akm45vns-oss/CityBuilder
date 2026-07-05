# Camera Architecture

The camera system in CityBuilder is designed specifically for RTS and City Building games, mimicking the feel of modern titles like Cities: Skylines and Planet Coaster.

## Rig Setup
The `CameraManager` relies on a strict 3-tier hierarchy to avoid Gimbal Lock and decouple translation from rotation:
1. **CameraRoot (Transform)**: Placed on the ground. Handles `X` and `Z` translation (panning), and `Y` axis rotation (yaw).
2. **CameraPivot (Transform)**: A child of CameraRoot. Handles `X` axis rotation (pitch/tilt) up and down.
3. **MainCamera (Camera)**: A child of CameraPivot. Handles local `Z` axis translation (zoom in/out).

## State Machine
The `CameraManager` implements the State Pattern (`ICameraState`) to cleanly transition between different input modes:
- **FreeCameraState**: The default gameplay state. Handles WASD, mouse drag panning, edge scrolling, and scrolling zoom.
- **FocusCameraState**: Smoothly lerps the camera to look at a specific `Transform`. Breaks back to `FreeCameraState` upon any keyboard input.
- **CinematicCameraState**: Reserved for future spline-based, UI-hidden cinematic flythroughs.
