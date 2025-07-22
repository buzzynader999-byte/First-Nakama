using System;
using Nakama;
using UnityEngine;

namespace _Scripts
{
    public class NakamaConnection : MonoBehaviour
    {
        private readonly string _scheme = "http";
        private readonly string _host = "127.0.0.1";
        private readonly int _port = 7350;
        private readonly string _key = "defaultkey";
        IClient _client;
        ISocket _socket;
        ISession _session;

        private string _ticket;

        async void Start()
        {
            try
            {
                _client = new Client(_scheme, _host, _port, _key, UnityWebRequestAdapter.Instance);
                _session = await _client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
                _socket = _client.NewSocket();
                await _socket.ConnectAsync(_session, true);

                _socket.ReceivedMatchmakerMatched += OnReceivedMatchMakerMatched;

                print(_session);
                print(_socket);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public async void FindMatch()
        {
            print("FindMatch");
            try
            {
                var matchmakingTicker = await _socket.AddMatchmakerAsync("*", 2, 2, null, null);
                _ticket = matchmakingTicker.Ticket;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        async void OnReceivedMatchMakerMatched(IMatchmakerMatched matchmaker)
        {
            try
            {
                print("Found matchmaker");
                var match = await _socket.JoinMatchAsync(matchmaker);
                print("joined matchmaker");
                print(match.Self.SessionId);
                foreach (var users in match.Presences)
                {
                    Debug.Log(users.SessionId);
                }
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }
    }
}