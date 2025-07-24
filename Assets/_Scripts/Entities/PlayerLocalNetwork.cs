using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Entities
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerHealthController))]
    public class PlayerLocalNetwork : MonoBehaviour
    {
        [SerializeField] private float syncFrequency = 0.05f;
        private float _syncTimer = 0;
        [SerializeField] PlayerInputController playerInputController;
        [SerializeField] Rigidbody2D _rigidbody2D;
        GameManager _gManager => GameManager.Instance;
        Vector2 _position => transform.position;
        Vector2 _velocity => _rigidbody2D.linearVelocity;

        private void LateUpdate()
        {
            _syncTimer += Time.deltaTime;
            if (_syncTimer >= syncFrequency)
            {
                SendMessage(OpCodes.VelocityAndPosition, MatchDataJson.VelocityAndPosition(_velocity, _position));
                _syncTimer = 0;
            }

            if (!playerInputController.IsInputChanged) return;
            SendMessage(OpCodes.Input, MatchDataJson.Input(playerInputController.InputDetails));
        }

        private void SendMessage(long op, string state) => _gManager.SendMatchState(op, state);
    }
}