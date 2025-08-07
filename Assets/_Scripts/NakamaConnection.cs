using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace _Scripts
{
    public class NakamaConnection : MonoBehaviour
    {
        private readonly string _scheme = "http";
        private readonly string _host = "localhost";
        [SerializeField] int _port = 7350;
        private readonly string _key = "defaultkey";
        IClient _client;
        public IClient Client => _client;
        ISocket _socket;
        public ISocket Socket => _socket;
        public static Action OnSocketCreated;
        public string UserName => _session.Username;
        public string UserId => _session.UserId;
        ISession _session;
        public ISession Session => _session;
        IMatch _match;
        private string _ticket;
        private string _matchID;

        public async Task Connect()
        {
            _client = new Client(_scheme, _host, _port, _key, UnityWebRequestAdapter.Instance);
            _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            _socket = _client.NewSocket();
            OnSocketCreated?.Invoke();
            await _socket.ConnectAsync(_session, true);

            _socket.ReceivedMatchmakerMatched += OnReceivedMatchMakerMatched;
            //_socket.ReceivedMatchState += OnReceivedMatchState;

            print(_session);
            print(_socket);
        }

        public async Task FindMatch()
        {
            print("FindMatch");
            if (!_socket.IsConnected)
            {
                _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
                await _socket.ConnectAsync(_session, true);
                Debug.Log("Reconnected: " + _socket.IsConnected);
            }

            var matchmakingTicker = await _socket.AddMatchmakerAsync("*", 2, 2, null, null);
            _ticket = matchmakingTicker.Ticket;
        }

        async void OnReceivedMatchMakerMatched(IMatchmakerMatched matchmaker)
        {
            try
            {
                print("Found matchmaker");
                _match = await _socket.JoinMatchAsync(matchmaker);
                _matchID = _match.Id;
                print("joined matchmaker");
                print(_match.Self.SessionId);
                foreach (var users in _match.Presences)
                {
                    Debug.Log(users.SessionId);
                }
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        public void CancelMatchMaking()
        {
            _socket.RemoveMatchmakerAsync(_ticket);
        }

        public void LeaveMatch()
        {
            if (_match != null && _socket != null)
                _socket.LeaveMatchAsync(_match);
        }

        public async Task SubmitScore(int newScore)
        {
            var r = await _client.WriteLeaderboardRecordAsync(_session, "attack", newScore);
        }
    }
}