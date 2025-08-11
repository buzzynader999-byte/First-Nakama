using System;
using _Scripts.Tools.Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            ScoreManager.onScoreChanged += OnScoreChanged;
            var score = Services.Get<ScoreManager>().CurrentScore;
            OnScoreChanged(Mathf.Max(0, score));
        }

        private void OnDisable() => ScoreManager.onScoreChanged -= OnScoreChanged;

        private void OnScoreChanged(int score)
        {
            text.text = "Score: " + score;
        }
        
    }
}