using System;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.Events
{
    /// <summary>
    /// A robust global event bus supporting typed events.
    /// Prevents memory leaks by requiring explicit unsubscription.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Subscribes to a specific event type.
        /// </summary>
        public static void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var currentAction))
            {
                _events[type] = Delegate.Combine(currentAction, action);
            }
            else
            {
                _events[type] = action;
            }
        }

        /// <summary>
        /// Unsubscribes from a specific event type.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var currentAction))
            {
                var newAction = Delegate.Remove(currentAction, action);
                if (newAction == null)
                {
                    _events.Remove(type);
                }
                else
                {
                    _events[type] = newAction;
                }
            }
        }

        /// <summary>
        /// Broadcasts an event to all subscribers.
        /// </summary>
        public static void Broadcast<T>(T eventData)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var currentAction))
            {
                (currentAction as Action<T>)?.Invoke(eventData);
            }
        }

        /// <summary>
        /// Clears all events. Useful for scene transitions or teardown.
        /// </summary>
        public static void Clear()
        {
            _events.Clear();
        }
    }
}
