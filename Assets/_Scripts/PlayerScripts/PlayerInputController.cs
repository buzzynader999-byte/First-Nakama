using UnityEngine;

namespace _Scripts.PlayerScripts
{
    public class PlayerInputController : MonoBehaviour
    {
        private bool _isInputChanged;
        private bool _isAttacked;
        public bool IsInputChanged => _isInputChanged;
        public bool IsAttacked => _isAttacked;
        [SerializeField] PlayerMovementController playerMovementController;
        [SerializeField] PlayerWeaponController weaponController;
        public PlayerInputControllerDetails InputDetails { set; get; } = new PlayerInputControllerDetails();
        PlayerInputControllerDetails _lastInputDetails = new PlayerInputControllerDetails();

        private void Update()
        {
            GetPlayerInput();

            _isInputChanged = CheckForInptChange(_lastInputDetails);
            if (!_isInputChanged)
                return;

            UpdateInputChanged();
            PerfoemInputChanged();
        }

        void GetPlayerInput()
        {
            _lastInputDetails.HorizontalInput = GatherInput.Controll.Player.Move.ReadValue<Vector2>().x;
            _lastInputDetails.Jump = GatherInput.Controll.Player.Jump.WasPressedThisFrame();
            _lastInputDetails.Attack = GatherInput.Controll.Player.Attack.WasPressedThisFrame();
        }

        private bool CheckForInptChange(PlayerInputControllerDetails details)
        {
            return !Mathf.Approximately(details.HorizontalInput, InputDetails.HorizontalInput) ||
                   details.Jump != InputDetails.Jump ||
                   details.Attack != InputDetails.Attack;
        }


        void UpdateInputChanged()
        {
            InputDetails.HorizontalInput = _lastInputDetails.HorizontalInput;
            InputDetails.Jump = _lastInputDetails.Jump;
            InputDetails.Attack = _lastInputDetails.Attack;
            _isAttacked = InputDetails.Attack;
        }

        void PerfoemInputChanged()
        {
            playerMovementController.SetHorizontal(InputDetails.HorizontalInput);
            if (_lastInputDetails.Attack)
                weaponController.Attack();

            if (_lastInputDetails.Jump)
                playerMovementController.Jump();
        }
    }

    public class PlayerInputControllerDetails
    {
        public float HorizontalInput { set; get; }

        public bool Jump { set; get; }

        public bool Attack { set; get; }
    }
}