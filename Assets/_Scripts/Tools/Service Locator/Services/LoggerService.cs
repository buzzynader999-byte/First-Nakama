using UnityEngine;

namespace _Scripts.Tools.Service_Locator.Services
{
    public class LoggerService : Service
    {
        public void Log(string message) => Debug.Log(message);
    }
}