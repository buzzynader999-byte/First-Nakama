using System;
using System.Threading.Tasks;
using _Scripts.Tools.Service_Locator;
using Nakama;
using UnityEngine;

namespace _Scripts.Managers
{
    public class NetworkManager : MonoService
    {
        [SerializeField] NakamaConnection connection;
        private UnityMainThreadDispatcher _mainThread;

        [SerializeField] private bool displayLogs;
        public static Action OnConnectedToNakama;

        public NakamaConnection Connection => connection;
        public static NetworkManager Instance;
        public ISocket Socket => connection.Socket;
        private string _ticket;
        private string _matchID;
        IMatch _match;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            connection.OnSocketCreated += SubscribeToSocket;
        }

        private void OnDisable()
        {
            connection.OnSocketCreated -= SubscribeToSocket;
        }

        private async void SocketOnClosed()
        {
            LogIt("closed");
            await TryConnect();
        }

        void SubscribeToSocket()
        {
            var mThread = GameManager.Instance.MainThread;
            Socket.Closed += () => mThread.Enqueue(SocketOnClosed);
            Socket.ReceivedError += (e) => mThread.Enqueue(() => SocketOnReceivedError(e));
            Socket.ReceivedMatchmakerMatched += (m) => mThread.Enqueue(() => OnReceivedMatchMakerMatched(m));
            Socket.ReceivedMatchPresence += (m) => mThread.Enqueue(() => SocketOnReceivedMatchPresence(m));
            Socket.ReceivedStatusPresence += (m) => mThread.Enqueue(() => SocketOnReceivedStatusPresence(m));
        }

        private void SocketOnReceivedError(Exception obj)
        {
            Debug.LogError("Error from socket :" + obj.Message);
        }

        async void OnReceivedMatchMakerMatched(IMatchmakerMatched matchmaker)
        {
            try
            {
                LogIt("Found matchmaker");
                _match = await Socket.JoinMatchAsync(matchmaker);
                LogIt("joined matchmaker");
                LogIt(_match.Self.SessionId);
                foreach (var users in _match.Presences)
                {
                    LogIt(users.SessionId);
                }

                _matchID = _match.Id;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void SocketOnReceivedMatchPresence(IMatchPresenceEvent presenceEvent)
        {
            foreach (var leave in presenceEvent.Leaves)
            {
                Debug.Log($"Player {leave.UserId} left the match.");
            }
        }

        private void SocketOnReceivedStatusPresence(IStatusPresenceEvent presenceEvent)
        {
            LogIt(presenceEvent);
            foreach (var joined in presenceEvent.Joins)
            {
                LogIt("User id '{0}' status joined '{1}'" + joined.UserId + joined.Status);
            }

            foreach (var left in presenceEvent.Leaves)
            {
                LogIt("User id '{0}' status left '{1}'" + left.UserId + left.Status);
            }
        }

        public async Task TryConnect()
        {
            try
            {
                LogIt("TryConnect ...");
                var connectingPopup = Services.Get<UIManager>().OpenConnectingPopup();

                var status = await connection.Connect();
                if (status)
                {
                    Services.Get<UIManager>().Close(connectingPopup);
                    Services.Get<UIManager>().OpenMainMenu();
                    OnConnectedToNakama?.Invoke();
                }
                else
                {
                    throw new Exception("Could not connect to nakama");
                }
            }
            catch (Exception e)
            {
                UIManager.Instance.OpenRetryConnection();
                Debug.LogError(e);
            }
        }

        public async Task FindMatch()
        {
            LogIt("FindMatch");
            _ticket = await Connection.FindMatch();
        }

        public void CancelMatchMaking()
        {
            Socket.RemoveMatchmakerAsync(_ticket);
        }

        public void LeaveMatch()
        {
            if (_match != null && Socket != null)
                Socket.LeaveMatchAsync(_match);
        }

        void LogIt(object message)
        {
            if (displayLogs)
            {
                Debug.Log(message);
            }
        }

        private void OnDestroy()
        {
            LeaveMatch();
        }

        protected override void Register()
        {
            Services.Register(this);
        }

        protected override void UnRegister()
        {
            Services.Unregister<NetworkManager>();
        }
    }
}