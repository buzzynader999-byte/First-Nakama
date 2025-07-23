using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Entities
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