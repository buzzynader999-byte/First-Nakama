using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Panel_MainMenu : Panel
    {
        public void FindMatch()
        {
            UIManager.Instance.FindMatch();
            UIManager.Instance.Close(this);
        }

        public void Exit()
        {
            UIManager.Instance.ExitGame();
        }

        public void OpenSettings()
        {
            UIManager.Instance.OpenSettings();
        }
    }
}