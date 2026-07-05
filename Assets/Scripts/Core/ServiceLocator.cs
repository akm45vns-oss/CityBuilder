using System;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.Core
{
    /// <summary>
    /// Global registry for services, preventing tight coupling between managers.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        /// <summary>
        /// Registers a service to the locator.
        /// </summary>
        public static void Register<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services.Add(type, service);
                Debug.Log($"[ServiceLocator] Registered {type.Name}");
            }
            else
            {
                Debug.LogError($"[ServiceLocator] Service {type.Name} is already registered!");
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

            Debug.LogError($"[ServiceLocator] Service {type.Name} not found!");
            return default;
        }

        /// <summary>
        /// Unregisters a service. Useful during scene teardown or testing.
        /// </summary>
        public static void Unregister<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
                Debug.Log($"[ServiceLocator] Unregistered {type.Name}");
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
