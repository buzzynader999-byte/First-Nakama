using UnityEngine;

namespace _Scripts.Tools.Service_Locator
{
    public abstract class MonoService : MonoBehaviour
    {
        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            UnRegister();
        }

        protected abstract void Register();
        protected abstract void UnRegister();
    }
}