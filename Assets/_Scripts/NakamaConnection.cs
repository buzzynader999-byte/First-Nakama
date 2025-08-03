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
        ISocket _socket;
        public ISocket Socket => _socket;
        public static Action OnSocketCreated;

        ISession _session;
        IMatch _match;
        private string _ticket;
        private string _matchID;

        public async Task Connect()
        {
            //var c2 = new Client(_scheme, _host, _port, _key, UnityWebRequestAdapter.Instance);
            //var c3 = new Client(_scheme, _host, _port, _key, UnityWebRequestAdapter.Instance);
            //var s2 = await c2.AuthenticateDeviceAsync("c222222222222222222222222222");
            //var s3 = await c3.AuthenticateDeviceAsync("c3333333333333333333");
            
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

        public async Task<int> GetScoreOfThisUser()
        {
            var leaderBoardScore = await _client.ListLeaderboardRecordsAsync(_session, "attack", null, null, 1);
            var records = new List<IApiLeaderboardRecord>(leaderBoardScore.Records.ToList());
            if (records.Count >= 1)
                if (!String.IsNullOrEmpty(records[0]?.Score))
                    return int.Parse(records[0].Score);
            return 0;
        }

        public async Task<List<IApiLeaderboardRecord>> GetLeaderboardRecords()
        {
            var leaderBoardScore = await _client.ListLeaderboardRecordsAsync(_session, "attack", null, null,100);
            var records = new List<IApiLeaderboardRecord>(leaderBoardScore.Records.ToList());
            return records;
        }
    }
}