using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Tools.Service_Locator
{
    public class ServiceLocator : MonoBehaviour
    {
        [SerializeField]List<Service> targetServices;
        private readonly Dictionary<Type, object> services = new();
        public static ServiceLocator Instance;
        private void Awake()
        {
            Instance = this;
            foreach (var target in targetServices)
            {
                services.Add(target.GetType(), target);
            }
        }

        public void Register<T>(T target) where T : Service => services.Add(target.GetType(), target);

        public void Unregister<T>() where T : Service => services.Remove(typeof(T));

        public T Get<T>() where T : Service
        {
            services.TryGetValue(typeof(T), out var target);
            return (T)target;
        }

        public void ClearAllServices() => services.Clear();
    }
}