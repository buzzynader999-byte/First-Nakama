using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;

namespace _Scripts.UI.Elements
{
    public class Panel_Game:Panel
    {
        public void OpenOptions()
        {
            Services.Get<UIManager>().Close(this);
            Services.Get<UIManager>().OpenOptionsInGame();
        }

        public void AddToScore()
        {
            Services.Get<ScoreManager>().AddScore(5);
        }

        public async void SubmitScore()
        {
            await Services.Get<ScoreManager>().SubmitScores();

        }
    }
}