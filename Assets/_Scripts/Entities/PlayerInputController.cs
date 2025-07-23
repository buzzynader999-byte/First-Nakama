using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Entities
{
    public class PlayerInputController : MonoBehaviour
    {
        private bool _isInputChanged;
        public bool IsInputChanged => _isInputChanged;
        public PlayerInputControllerDetails InputDetails { set; get; } = new PlayerInputControllerDetails();

        private void Update()
        {
            var horizontal = GatherInput.Controll.Player.Move.ReadValue<Vector2>().x;
            print(horizontal);
            //var jump = GatherInput.Controll.Player.Jump.ReadValue<Vector2>();

            _isInputChanged = CheckForInptChange(horizontal);

            InputDetails.HorizontalInput = horizontal;
        }

        private bool CheckForInptChange(float horizontal)
        {
            return horizontal != InputDetails.HorizontalInput;
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