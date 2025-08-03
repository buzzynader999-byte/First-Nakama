using UnityEngine;

namespace _Scripts.Tools.Service_Locator.Services
{
    public class AudioService : Service
    {
        public void PlaySound(string sound) => Debug.Log(sound + " played");
    }
}