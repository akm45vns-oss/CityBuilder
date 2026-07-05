namespace CityBuilder.Tools
{
    public interface ITool
    {
        void OnEnable();
        void OnDisable();
        void OnUpdate();
    }
}
