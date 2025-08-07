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
    public class ScoreManager : Service
    {
        private NakamaConnection connection;
        public static Action<int> onScoreChanged;
        public static Action<int> onServerScoreChanged;
        public int CurrentScore { get; private set; }
        public int ScoreInServer { get; private set; }

        private void OnEnable() => PlayerLocalNetwork.onPlayerAttacked += OnPlayerAttacked;

        private void OnDisable() => PlayerLocalNetwork.onPlayerAttacked -= OnPlayerAttacked;

        private async void Start()
        {
            connection = GameManager.Instance.NakamaConnection;
            var loadedScore = await LeaderBoardInterface.GetScoreOfThisUser(connection, "attack");
            onServerScoreChanged?.Invoke(loadedScore);
        }

        private void OnPlayerAttacked()
        {
            CurrentScore += 5;
            print("Score in local: " + CurrentScore);
            onScoreChanged?.Invoke(CurrentScore);
        }

        public async Task SubmitScores()
        {
            print("Trying to submit scores");
            try
            {
                await connection.SubmitScore(CurrentScore);
                ScoreInServer = await LeaderBoardInterface.GetScoreOfThisUser(connection, "attack");
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
            var records = await LeaderBoardInterface.GetRecords(connection, leaderBoardId, null, null, limit);
            return records;
        }

        public async Task<IApiLeaderboardRecord> GetPlayerRank(string leaderboardId)
        {
            string ownerId = connection.UserId;
            var result = await connection.Client.ListLeaderboardRecordsAroundOwnerAsync(
                connection.Session,
                leaderboardId,
                connection.UserId,
                limit: 1,
                expiry: null
            );

            // پیدا کردن رنک خودتون
            return result.OwnerRecords.FirstOrDefault();
        }
    }
}