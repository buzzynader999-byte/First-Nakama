using System;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "New Nakama connection", menuName = "SO/Nakama Connection", order = 0)]
    public class NakamaConnection : ScriptableObject
    {
        [SerializeField] private string scheme = "http";
        [SerializeField] private string host = "localhost";
        public string Host { set; get; }
        [SerializeField] private int port = 7350;
        [SerializeField] private string key = "defaultkey";
        [SerializeField] private bool displayLogs;

        public IClient Client => _client;
        IClient _client;
        public ISocket Socket => _socket;
        ISocket _socket;
        public ISession Session => _session;
        ISession _session;
        IMatch _match;
        public string UserId => _session.UserId;
        public string UserName => _session.Username;
        public static Action OnSocketCreated;

        private string _ticket;
        private string _matchID;

        public async Task<bool> Connect()
        {
            try
            {
                _client = new Client(scheme, host, port, key, UnityWebRequestAdapter.Instance);
                _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
                if (_session == null || _session.IsExpired)
                {
                    Debug.LogError("Authentication failed or session is expired");
                    return false;
                }

                _socket = _client.NewSocket();
                if (_socket == null)
                {
                    Debug.LogError("Failed to create socket");
                    return false;
                }

                OnSocketCreated?.Invoke();
                _socket.ReceivedMatchmakerMatched += OnReceivedMatchMakerMatched;
                //_socket.ReceivedMatchState += OnReceivedMatchState;

                await _socket.ConnectAsync(_session, appearOnline: true);
                Debug.Log($"Session: {_session}");
                Debug.Log($"Socket: {_socket}");

                return _socket.IsConnected;
            }
            catch (Exception ex)
            {
                Debug.Log($"Connection failed: {ex.Message}");
                return false;
            }
        }

        public async Task FindMatch()
        {
            LogIt("FindMatch");
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
                LogIt("Found matchmaker");
                _match = await _socket.JoinMatchAsync(matchmaker);
                _matchID = _match.Id;
                Debug.Log("joined matchmaker");
                Debug.Log(_match.Self.SessionId);
                foreach (var users in _match.Presences)
                {
                    LogIt(users.SessionId);
                }
            }
            catch (Exception e)
            {
                LogIt(e.Message);
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

        void LogIt(object message)
        {
            if (displayLogs)
            {
                Debug.Log(message);
            }
        }
    }
}