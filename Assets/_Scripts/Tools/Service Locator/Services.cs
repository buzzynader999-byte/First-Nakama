using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Tools.Service_Locator
{
    public class Services
    {
        private static readonly Dictionary<Type, object> services = new();
        public static void Register<T>(T target) where T : MonoService => services.Add(target.GetType(), target);

        public static void Unregister<T>() where T : MonoService => services.Remove(typeof(T));

        public static T Get<T>() where T : MonoService
        {
            services.TryGetValue(typeof(T), out var target);
            return (T)target;
        }

        public static void ClearAllServices() => services.Clear();
    }
}