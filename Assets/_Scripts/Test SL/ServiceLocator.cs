using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace _Scripts.Test_SL
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static void Register<T>(T target) where T : IService => Services.Add(target.GetType(), target);

        public static void Unregister<T>() where T : IService => Services.Remove(typeof(T));

        public static T Get<T>() where T : IService
        {
            Services.TryGetValue(typeof(T), out var target);
            return (T)target;
        }

        public static void ClearAllServices() => Services.Clear();
    }
}