using System;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigidbody2D;
        private float _horizontal;

        private void FixedUpdate()
        {
            PerformMovement();
        }

        private void PerformMovement()
        {
           rigidbody2D.linearVelocity = Vector2.right * _horizontal;
        }

        public void SetHorizontal(float horizontal)
        {
            _horizontal = horizontal;
        }
    }
}