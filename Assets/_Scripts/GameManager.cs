using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Entities;
using Nakama;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] NakamaConnection nakamaConnection;
        [SerializeField] MainMenu mainMenu;
        public NakamaConnection NakamaConnection => nakamaConnection;
        private ISocket _socket => nakamaConnection.Socket;
        private IUserPresence _localUser;
        private IUserPresence _localPlayer;
        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
        private IMatch _currentMatch;
        public static GameManager Instance;

        private void Awake()
        {
            Instance = this;
            nakamaConnection.Connect();
        }

        private void Start()
        {
            var mainThread = UnityMainThreadDispatcher.Instance();
            nakamaConnection.Socket.ReceivedMatchmakerMatched +=
                m => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
            nakamaConnection.Socket.ReceivedMatchPresence += m => mainThread.Enqueue(() => OnReceivedMatchPresence(m));
            nakamaConnection.Socket.ReceivedMatchState += m => mainThread.Enqueue(() => OnReceivedMatchState(m));
        }

        private void OnReceivedMatchState(IMatchState matchState)
        {
            //...
            Debug.Log("Received match state");
            switch (matchState.OpCode)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            foreach (var joinedOne in matchPresenceEvent.Joins)
            {
                SpawnPlayer(matchPresenceEvent.MatchId, joinedOne);
            }

            foreach (var leaved in matchPresenceEvent.Leaves)
            {
                Destroy(_players[leaved.SessionId]);
                _players.Remove(leaved.SessionId);
            }
        }

        private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmaker)
        {
            try
            {
                _localUser = matchmaker.Self.Presence;
                print("Found matchmaker");
                var match = await _socket.JoinMatchAsync(matchmaker);
                print("joined matchmaker");
                mainMenu.DeActive();
                print(match.Self.SessionId);
                foreach (var user in match.Presences)
                {
                    SpawnPlayer(match.Id, user);
                }

                _currentMatch = match;
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        void SpawnPlayer(string matchID, IUserPresence targetUser)
        {
            if (_players.ContainsKey(targetUser.SessionId))
                return;
            var isLocalUser = _localUser.SessionId != targetUser.SessionId;
            var newUser = PlayerFactory.Instance.GetNewPlayer(isLocalUser);
            if (!isLocalUser)
                newUser.GetComponent<PlayerRemote>().NetworkData = new RemotePlayerNetworkData(matchID, targetUser);
            else
            {
                _localPlayer = _localUser;
                newUser.GetComponent<PlayerHealthController>().PlayerDeath += PlayerDeath;
            }

            _players.Add(targetUser.SessionId, newUser);
        }

        private void PlayerDeath(GameObject targetPlayer)
        {
            Debug.Log("Player Dead");
            SendMatchState(OpCodes.Died, MatchDataJson.Died(targetPlayer.transform.position));
        }

        public void FindMatch()
        {
            nakamaConnection.FindMatch();
        }

        public void SendMatchState(long op, string state) => _socket.SendMatchStateAsync(_currentMatch.Id, op, state);

        public async void SendTestMatchState()
        {
            try
            {
                await _socket.SendMatchStateAsync(_currentMatch.Id, OpCodes.Died, "");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}