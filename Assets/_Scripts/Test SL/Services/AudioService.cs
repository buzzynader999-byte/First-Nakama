using UnityEngine;

namespace _Scripts.Test_SL.Services
{
    public class AudioService : IService
    {
        public void PlaySound(string sound) => Debug.Log(sound + " played");
    }
}