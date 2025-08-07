using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.PlayerScripts
{
    public class PlayerHealthController : MonoBehaviour
    {
        public UnityAction<GameObject> PlayerDeath { set; get; }
        
        
        void OnDead()
        {
            //...
            PlayerDeath?.Invoke(gameObject);
        }
    }
}