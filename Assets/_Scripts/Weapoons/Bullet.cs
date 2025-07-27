using UnityEngine;

namespace _Scripts.Weapoons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody;
        public Rigidbody2D Rigidbody2D => rigidbody;
    }
}