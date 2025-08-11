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
        [SerializeField] private int port = 7350;
        [SerializeField] private string key = "defaultkey";

        public string Host
        {
            set => host = value;
            get => host;
        }

        public IClient Client => _client;
        IClient _client;
        public ISocket Socket => _socket;
        ISocket _socket;
        public ISession Session => _session;
        ISession _session;
        public string UserId => _session.UserId;
        public string UserName => _session.Username;
        public Action OnSocketCreated;
        private const string SessionPrefName = "nakama.session";
        private const string DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";

        public async Task<bool> Connect()
        {
            try
            {
                _client = new Client(scheme, host, port, key, UnityWebRequestAdapter.Instance);
                var authToken = PlayerPrefs.GetString(SessionPrefName);
                if (!string.IsNullOrEmpty(authToken))
                {
                    var session = Nakama.Session.Restore(authToken);
                    if (!session.IsExpired)
                    {
                        _session = session;
                    }
                }

                if (_session == null)
                {
                    string deviceId = GetDeviceId();
                    _session = await Client.AuthenticateDeviceAsync(deviceId);
                    PlayerPrefs.SetString(SessionPrefName, _session.AuthToken);
                }
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

        public async Task<string> FindMatch()
        {
            if (!_socket.IsConnected)
            {
                _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
                await _socket.ConnectAsync(_session, true);
                Debug.Log("Reconnected: " + _socket.IsConnected);
            }

            var matchmakingTicker = await _socket.AddMatchmakerAsync("*", 2, 2, null, null);
            return matchmakingTicker.Ticket;
        }

        public async Task SubmitScore(int newScore)
        {
            var r = await _client.WriteLeaderboardRecordAsync(_session, "attack", newScore);
        }

        string GetDeviceId()
        {
            string deviceId;
            if (PlayerPrefs.HasKey(DeviceIdentifierPrefName))
            {
                deviceId = PlayerPrefs.GetString(DeviceIdentifierPrefName);
            }
            else
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                if (deviceId == SystemInfo.unsupportedIdentifier)
                {
                    deviceId = System.Guid.NewGuid().ToString();
                }
                PlayerPrefs.SetString(DeviceIdentifierPrefName, deviceId);
            }

            return deviceId;
        }
    }
}