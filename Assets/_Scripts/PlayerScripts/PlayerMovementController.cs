using System;
using _Scripts.PlayerScripts;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] float movementSpeed;
        [SerializeField] Animator bodyAnimator;
        [SerializeField] Animator finAnimator;

        PlayerInputControllerDetails _inputDetails = new();

        private float jumpTimer;
        private float groundedTimer;
        private float JumpForce;
        //private bool isGrounded;

        private void FixedUpdate()
        {
            PerformMovement();
            //PerformJump();
        }

        private void PerformMovement()
        {
            rigidbody2D.linearVelocity =
                new Vector2(_inputDetails.HorizontalInput * movementSpeed, rigidbody2D.linearVelocity.y);
        }

        public void SetHorizontal(float horizontal)
        {
            _inputDetails.HorizontalInput = horizontal;
            if (horizontal == 0)
            {
                //bodyAnimator.SetBool("Idle", true);
                //bodyAnimator.Play("FishIdle");
                return;
            }

            /*if (bodyAnimator.GetBool("FishRun") == false)
            {
                bodyAnimator.Play("FishRun");
                bodyAnimator.SetBool("Run", true);
            }*/

            var rotationY = transform.rotation.y;
            if (_inputDetails.HorizontalInput > 0.1f)
                rotationY = 0;
            else if (_inputDetails.HorizontalInput < -0.1f) rotationY = 180;

            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }

        public void SetJUmp(bool value)
        {
            _inputDetails.Jump = value;
            rigidbody2D.AddForce(Vector2.up * 100, ForceMode2D.Impulse);
            bodyAnimator.Play("");
        }
    }
}