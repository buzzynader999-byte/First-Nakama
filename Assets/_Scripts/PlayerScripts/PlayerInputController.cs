using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Entities
{
    public class PlayerInputController : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Jump = Animator.StringToHash("Jump");
        [SerializeField] private Animator bodyAnimator;
        private bool _isInputChanged;
        public bool IsInputChanged => _isInputChanged;
        [SerializeField] PlayerMovementController playerMovementController;
        public PlayerInputControllerDetails InputDetails { set; get; } = new PlayerInputControllerDetails();
        PlayerInputControllerDetails _lastInputDetails = new PlayerInputControllerDetails();

        private void Update()
        {
            _lastInputDetails.HorizontalInput = GatherInput.Controll.Player.Move.ReadValue<Vector2>().x;
            _lastInputDetails.Jump = GatherInput.Controll.Player.Jump.IsPressed();

            _isInputChanged = CheckForInptChange(_lastInputDetails);
            if (!_isInputChanged)
                return;

            bodyAnimator.SetFloat(Horizontal, Mathf.Abs(_lastInputDetails.HorizontalInput));
            InputDetails.HorizontalInput = _lastInputDetails.HorizontalInput;
            InputDetails.Jump = _lastInputDetails.Jump;
            playerMovementController.SetHorizontal(InputDetails.HorizontalInput);
            if (_lastInputDetails.Jump)
                playerMovementController.Jump();
        }

        private bool CheckForInptChange(PlayerInputControllerDetails details)
        {
            return !Mathf.Approximately(details.HorizontalInput, InputDetails.HorizontalInput) ||
                   details.Jump != InputDetails.Jump;
        }
    }

    public class PlayerInputControllerDetails
    {
        public float HorizontalInput { set; get; }

        public bool Jump { set; get; }

        public bool JumpHeld { set; get; }

        public bool Attack { set; get; }
    }
}