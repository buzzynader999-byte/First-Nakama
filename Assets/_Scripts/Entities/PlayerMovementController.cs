using System;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigidbody2D;
        private float _horizontal;
        [SerializeField] private float moveSpeed;

        private void FixedUpdate()
        {
            PerformMovement();
        }

        private void PerformMovement()
        {
           rigidbody2D.linearVelocity =  new Vector2(_horizontal * moveSpeed, rigidbody2D.linearVelocity.y);
        }

        public void SetHorizontal(float horizontal)
        {
            _horizontal = horizontal;
        }
    }
}