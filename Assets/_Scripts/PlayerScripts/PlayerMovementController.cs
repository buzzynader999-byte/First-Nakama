using System;
using System.Collections;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerMovementController : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
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
            bodyAnimator.SetFloat(Horizontal,Mathf.Abs(horizontal));
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
                bodyAnimator.SetTrigger(Jump1);
                rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void LateUpdate()
        {
            if (_jumped && rigidbody2D.linearVelocity.normalized.y < 0 && !_falling)
            {
                bodyAnimator.SetTrigger(Fall);
                _falling = true;
            }

            if (_falling && rigidbody2D.linearVelocity.normalized.y == 0)
            {
                bodyAnimator.SetTrigger(Land);
                _jumped = false;
                _falling = false;
            }
        }
    }
}