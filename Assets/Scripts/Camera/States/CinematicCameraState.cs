namespace CityBuilder.Camera
{
    public class CinematicCameraState : ICameraState
    {
        private CameraManager _cam;

        public CinematicCameraState(CameraManager cam)
        {
            _cam = cam;
        }

        public void Enter()
        {
            // Hide UI, setup spline path
        }

        public void Exit()
        {
            // Restore UI
        }

        public void UpdateState()
        {
            // Move along spline
        }
    }
}
