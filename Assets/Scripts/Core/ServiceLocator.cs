using System;
using System.Collections.Concurrent;
using CityBuilder.Core.Logging;

namespace CityBuilder.Core
{
    /// <summary>
    /// Thread-safe global registry for services, preventing tight coupling between managers.
    /// Uses ConcurrentDictionary to ensure safe concurrent operations from worker threads.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly ConcurrentDictionary<Type, IService> _services = new ConcurrentDictionary<Type, IService>();

        /// <summary>
        /// Registers a service to the locator.
        /// </summary>
        public static void Register<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (_services.TryAdd(type, service))
            {
                GameLogger.Info($"[ServiceLocator] Registered {type.Name}");
            }
            else
            {
                GameLogger.Error($"[ServiceLocator] Service {type.Name} is already registered!");
            }
        }

        /// <summary>
        /// Resolves and returns a registered service.
        /// </summary>
        public static T Get<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            GameLogger.Error($"[ServiceLocator] Service {type.Name} not found!");
            return default;
        }

        /// <summary>
        /// Unregisters a service. Useful during scene teardown or testing.
        /// </summary>
        public static void Unregister<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.TryRemove(type, out _))
            {
                GameLogger.Info($"[ServiceLocator] Unregistered {type.Name}");
            }
        }
        
        /// <summary>
        /// Clears all registered services.
        /// </summary>
        public static void Clear()
        {
            _services.Clear();
        }
    }
}
