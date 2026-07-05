namespace CityBuilder.Core
{
    /// <summary>
    /// Base interface for all core managers/services in the application.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Initializes the service. 
        /// </summary>
        void Initialize();
    }
}
