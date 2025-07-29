using System;
using System.Collections;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] float jumpForce = 200;
        [SerializeField] float movementSpeed;
        [SerializeField] Animator bodyAnimator;
        private float _horizontal;

        bool _jumped = false;
        bool _falling = false;

        private void FixedUpdate()
        {
            PerformMovement();
        }

        private void PerformMovement()
        {
            rigidbody2D.linearVelocity = new Vector2(_horizontal * movementSpeed, rigidbody2D.linearVelocity.y);
        }

        public void SetHorizontal(float horizontal)
        {
            _horizontal = horizontal;

            if (_horizontal == 0) return;
            float rotation = 0;
            if (_horizontal >= 0.1f)
            {
                rotation = 0;
            }
            else if (_horizontal <= -0.1f)
            {
                rotation = 180;
            }

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        public void Jump()
        {
            if (!_jumped)
            {
                _jumped = true;
                bodyAnimator.SetTrigger("Jump");
                rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void LateUpdate()
        {
            if (_jumped && rigidbody2D.linearVelocity.normalized.y < 0 && !_falling)
            {
                bodyAnimator.SetTrigger("Fall");
                _falling = true;
            }

            if (_falling && rigidbody2D.linearVelocity.normalized.y == 0)
            {
                bodyAnimator.SetTrigger("Land");
                _jumped = false;
                _falling = false;
            }
        }
    }
}