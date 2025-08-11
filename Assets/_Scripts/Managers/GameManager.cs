using System;
using _Scripts.PlayerScripts;
using _Scripts.Tools;
using _Scripts.Tools.Service_Locator;
using Nakama;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private NetworkManager Network => NetworkManager.Instance;
        private IUserPresence _localUser;
        private IMatch _currentMatch;
        public static GameManager Instance;
        private UnityMainThreadDispatcher _mainThread;
        public UnityMainThreadDispatcher MainThread => _mainThread;

        private async void Awake()
        {
            Instance = this;
            await Network.TryConnect();
        }

        private void OnEnable()
        {
            _mainThread = UnityMainThreadDispatcher.Instance();
            Network.Connection.OnSocketCreated += OnSocketCreated;
        }

        private void OnSocketCreated()
        {
            Network.Socket.ReceivedMatchmakerMatched +=
                m => _mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
            Network.Socket.ReceivedMatchPresence += m => _mainThread.Enqueue(() => OnReceivedMatchPresence(m));
            Network.Socket.ReceivedMatchState += m => _mainThread.Enqueue(() => OnReceivedMatchState(m));
        }

        private void OnReceivedMatchState(IMatchState matchState)
        {
            //...
            switch (matchState.OpCode)
            {
                case 1:
                    break;
                case 2: //InputChanged
                    print("Input state received");
                    break;
                case 3:
                    break;
                case 4: //Respawned
                    break;
                case 5: //Leaved
                    break;
            }
        }


        private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            foreach (var joinedOne in matchPresenceEvent.Joins)
            {
                print(joinedOne.SessionId + " joined");
                SpawnPlayer(matchPresenceEvent.MatchId, joinedOne);
            }

            foreach (var leaved in matchPresenceEvent.Leaves)
            {
                print(leaved.SessionId + " leaved");
                PlayerSpawner.instance.Destroy(leaved.SessionId);
            }
        }

        private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmaker)
        {
            try
            {
                _localUser = matchmaker.Self.Presence;
                print("Found matchmaker");
                var match = await Network.Socket.JoinMatchAsync(matchmaker);
                print("joined matchmaker");
                Services.Get<UIManager>().OpenGameMenu();
                //print(match.Self.SessionId);
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

        async void SpawnPlayer(string matchID, IUserPresence targetUser)
        {
            try
            {
                var isLocalUser = _localUser.SessionId == targetUser.SessionId;
                var newUser = await PlayerSpawner.instance.SpawnPlayerAsync(targetUser, isLocalUser);
                if (!isLocalUser)
                {
                    newUser.GetComponent<PlayerRemote>().NetworkData = new RemotePlayerNetworkData(matchID, targetUser);
                }
                else
                {
                    newUser.GetComponent<PlayerHealthController>().PlayerDeath += PlayerDeath;
                    //...
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void PlayerDeath(GameObject targetPlayer)
        {
            SendMatchState(OpCodes.Died, MatchDataJson.Died(targetPlayer.transform.position));
        }

        public async void FindMatch()
        {
            Services.Get<UIManager>().OpenFindMatch();
            await Network.FindMatch();
        }

        public void SendMatchState(long op, string state) =>
            Network.Socket.SendMatchStateAsync(_currentMatch.Id, op, state);

        public void CancelMatchMaking()
        {
            Network.CancelMatchMaking();
            Services.Get<UIManager>().OpenMainMenu();
        }

        public void LeaveMatch()
        {
            Network.LeaveMatch();
            Services.Get<UIManager>().OpenMainMenu();
        }
    }
}