using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.LeaderBoard;
using _Scripts.Managers;
using _Scripts.PlayerScripts;
using _Scripts.Tools.Service_Locator;
using Nakama;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class ScoreManager : MonoService
    {
        private NakamaConnection _connection;
        public static Action<int> onScoreChanged;
        public static Action<int> onServerScoreChanged;
        public int CurrentScore { get; private set; }
        public int ScoreInServer { get; private set; }

        private void OnEnable()
        {
            PlayerLocalNetwork.onPlayerAttacked += OnPlayerAttacked;
            NetworkManager.OnConnectedToNakama += OnConnectedToNakama;
        }

        private void OnDisable()
        {
            PlayerLocalNetwork.onPlayerAttacked -= OnPlayerAttacked;
        }
        private async void OnConnectedToNakama()
        {
            try
            {
                _connection = NetworkManager.Instance.Connection;
                var loadedScore = await LeaderBoardInterface.GetScoreOfThisUser(_connection, "attack");
                onServerScoreChanged?.Invoke(loadedScore);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void OnPlayerAttacked()
        {
            AddScore(5);
        }

        public void AddScore(int score)
        {
            CurrentScore += score;
            print("Score in local: " + CurrentScore);
            onScoreChanged?.Invoke(CurrentScore);
        }

        public async Task SubmitScores()
        {
            print("Trying to submit scores");
            try
            {
                await _connection.SubmitScore(CurrentScore);
                ScoreInServer = await LeaderBoardInterface.GetScoreOfThisUser(_connection, "attack");
                onServerScoreChanged?.Invoke(ScoreInServer);
            }
            catch (Exception e)
            {
                Debug.LogAssertion(e);
            }
        }

        private void Update()
        {
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                _ = SubmitScores();
            }
        }

        public async Task<List<IApiLeaderboardRecord>> GetRecords(string leaderBoardId, int limit)
        {
            var records = await LeaderBoardInterface.GetRecords(_connection, leaderBoardId, null, null, limit);
            return records;
        }
        public async Task<IApiLeaderboardRecord> GetPlayerRank(string leaderboardId)
        {
            string ownerId = _connection.UserId;
            var result = await _connection.Client.ListLeaderboardRecordsAroundOwnerAsync(
                _connection.Session,
                leaderboardId,
                _connection.UserId,
                limit: 1,
                expiry: null
            );

            // پیدا کردن رنک خودتون
            return result.OwnerRecords.FirstOrDefault();
        }
        protected override void Register()
        {
            Services.Register(this);
        }

        protected override void UnRegister()
        {
            Services.Unregister<ScoreManager>();
        }
    }
}