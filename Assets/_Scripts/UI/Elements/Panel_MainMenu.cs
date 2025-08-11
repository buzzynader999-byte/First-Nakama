using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Panel_MainMenu : Panel
    {
        public void FindMatch()
        {
            Services.Get<UIManager>().Close(this);
            GameManager.Instance.FindMatch();
        }

        public void OpenLeaderBoard()
        {
            Services.Get<UIManager>().OpenLeaderBoard();
        }
        public void Exit()
        {
            Services.Get<UIManager>().ExitGame();
        }
        public void AddToScore()
        {
            Services.Get<ScoreManager>().AddScore(5);
        }

        public async void SubmitScore()
        {
            await Services.Get<ScoreManager>().SubmitScores();
        }

        public void OpenProfile()
        {
            Services.Get<UIManager>().OpenProfile();

        }
    }
}