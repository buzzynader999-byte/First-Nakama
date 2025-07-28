using _Scripts.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.PlayerScripts
{
    public class PlayerInputController : MonoBehaviour
    {
        private bool _isInputChanged;
        public bool IsInputChanged => _isInputChanged;
        [SerializeField] PlayerMovementController playerMovementController;
        [SerializeField] PlayerWeaponController playerWeaponController;
        public PlayerInputControllerDetails InputDetails { set; get; } = new();
        PlayerInputControllerDetails _lastInput = new();

        private void Update()
        {
            _lastInput.HorizontalInput = GatherInput.Controll.Player.Move.ReadValue<Vector2>().x;
            _lastInput.Jump = Keyboard.current.spaceKey.wasPressedThisFrame;
            _lastInput.Attack = Mouse.current.leftButton.wasPressedThisFrame;

            _isInputChanged = CheckForInptChange(_lastInput);
            if (!_isInputChanged)
            {
                return;
            }

            InputDetails.HorizontalInput = _lastInput.HorizontalInput;
            InputDetails.Jump = _lastInput.Jump;
            InputDetails.Attack = _lastInput.Attack;

            playerMovementController.SetHorizontal(InputDetails.HorizontalInput);
            playerMovementController.SetJUmp(InputDetails.Jump);
            if (InputDetails.Attack)
                playerWeaponController.Attack();
        }

        private bool CheckForInptChange(PlayerInputControllerDetails input)
        {
            return !Mathf.Approximately(input.HorizontalInput, InputDetails.HorizontalInput) ||
                   input.Jump != InputDetails.Jump ||
                   input.Attack != InputDetails.Attack;
        }
    }

    public class PlayerInputControllerDetails
    {
        public float HorizontalInput { set; get; }

        public bool Jump { set; get; }

        public bool Attack { set; get; }
    }
}