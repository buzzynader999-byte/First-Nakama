using System;
using System.Collections.Generic;
using System.Text;
using _Scripts.Managers;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace _Scripts.Entities
{
    [RequireComponent(typeof(PlayerHealthController))]
    public class PlayerRemote : MonoBehaviour
    {
        GameManager _gameManager => GameManager.Instance;
        public RemotePlayerNetworkData NetworkData { set; get; }
        [SerializeField] float lerpTime = 0.2f;
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] PlayerMovementController movementController;
        float _lerpTimer = 0;
        private Vector3 _targetPosition;
        private Vector3 _lastPosition;

        private void Start()
        {
            _gameManager.NakamaConnection.Socket.ReceivedMatchState += EnqueueOnReceivedMatchState;
        }

        private void EnqueueOnReceivedMatchState(IMatchState matchState)
        {
            var mainThread = UnityMainThreadDispatcher.Instance();
            mainThread.Enqueue(() => OnReceivedMatchState(matchState));
        }

        private void OnReceivedMatchState(IMatchState matchState)
        {
            if (matchState.UserPresence.SessionId != NetworkData.User.SessionId) return;

            switch (matchState.OpCode)
            {
                case OpCodes.Died:
                    Debug.Log("Died");
                    break;
                case OpCodes.Input:
                    SetInputFromState(matchState.State);
                    //print("input from remote");
                    break;
                case OpCodes.VelocityAndPosition:
                    UpdateVelocityAndPositionFromState(matchState.State);
                    break;
            }
        }

        private void UpdateVelocityAndPositionFromState(byte[] state)
        {
            var stateDictionary = GetStateAsDictionary(state);

            rigidbody2D.linearVelocity = new Vector2(float.Parse(stateDictionary["velocity.x"]),
                float.Parse(stateDictionary["velocity.y"]));

            var position = new Vector3(
                float.Parse(stateDictionary["position.x"]),
                float.Parse(stateDictionary["position.y"]),
                0);
            _lastPosition = transform.position;
            _targetPosition = Vector3.Lerp(transform.position, position, lerpTime / _lerpTimer);
        }

        private IDictionary<string, string> GetStateAsDictionary(byte[] state) =>
            Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();

        private void SetInputFromState(byte[] state)
        {
            var stateDictionary = GetStateAsDictionary(state);
            movementController.SetHorizontal(float.Parse(stateDictionary["horizontalInput"]));
        }

        void LateUpdate()
        {
            _lerpTimer += Time.deltaTime;
            if (_lerpTimer >= lerpTime)
            {
                //...
                _lerpTimer = lerpTime;
            }

            if (_lerpTimer >= lerpTime)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpTimer / lerpTime);
            }
        }

        private void OnDestroy()
        {
            if (_gameManager != null)
            {
                _gameManager.NakamaConnection.Socket.ReceivedMatchState -= EnqueueOnReceivedMatchState;
            }
        }
    }
}