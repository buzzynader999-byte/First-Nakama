using System;
using System.Diagnostics.Tracing;
using _Scripts.Entities;
using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using _Scripts.Weapoons;
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

        public void SubmitScores()
        {
            connection.SubmitScore(CurrentScore);
            ScoreInServer = connection.GetScoreOfThisUser().Result;
            onServerScoreChanged?.Invoke(ScoreInServer);
        }
        private void Update()
        {
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                SubmitScores();
            }
        }
    }
}