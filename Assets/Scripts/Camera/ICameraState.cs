namespace CityBuilder.Camera
{
    /// <summary>
    /// Base interface for Camera State Machine.
    /// </summary>
    public interface ICameraState
    {
        void Enter();
        void UpdateState();
        void Exit();
    }
}
