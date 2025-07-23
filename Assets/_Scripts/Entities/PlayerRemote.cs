using System;
using Nakama;
using UnityEngine;

namespace _Scripts.Entities
{
    public class PlayerRemote : MonoBehaviour
    {
        GameManager _gameManager => GameManager.Instance;
        public RemotePlayerNetworkData NetworkData { set; get; }
        [SerializeField] float lerpTime = 0.2f;
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] PlayerMovementController movementController;
        float _lerpTimer = 0;

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
                    Debug.Log("Input");
                    break;
                case OpCodes.VelocityAndPosition:
                    break;
            }
        }

        void LateUpdate()
        {
            _lerpTimer += Time.deltaTime;
            if (_lerpTimer >= lerpTime)
            {
                //...
                _lerpTimer = lerpTime;
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