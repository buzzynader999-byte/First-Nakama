using UnityEngine;

namespace _Scripts.Test_SL.Services
{
    public class LoggerService : IService
    {
        public void Log(string message) => Debug.Log(message);
    }
}