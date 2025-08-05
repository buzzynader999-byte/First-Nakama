using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Entities;
using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using _Scripts.Weapoons;
using Nakama;
using TMPro;
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
            var loadedScore = await connection.GetScoreOfThisUser();
            onServerScoreChanged?.Invoke(loadedScore);
            print("Current ServerScore : " + loadedScore);
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
                /* if (connection.Socket?.IsConnected == false)
                 {
                     Debug.LogWarning("Socket not connected, attempting to reconnect...");
                     await connection.Connect();
                 }
 */
                await connection.SubmitScore(CurrentScore);
                ScoreInServer = await connection.GetScoreOfThisUser();
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

        public async Task<List<IApiLeaderboardRecord>> GetRecords(string leaderBoardId,int limit)
        {
            var records = await connection.GetLeaderboardRecords(leaderBoardId,limit);
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