using _Scripts.Tools.Service_Locator.Services;
using UnityEngine;

namespace _Scripts.Tools.Service_Locator
{
    public class ExampleUse : MonoBehaviour
    {
        private LoggerService _logger;

        private void Awake()
        {
            ServiceLocator.Register(new AudioService());
            ServiceLocator.Register(new LoggerService());
            
            _logger = ServiceLocator.Get<LoggerService>();
            _logger.Log(Time.frameCount.ToString());

        }
    }
}