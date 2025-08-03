using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Panel_MainMenu : Panel
    {
        public void FindMatch()
        {
            ServiceLocator.Instance.Get<UIManager>().Close(this);
            GameManager.Instance.FindMatch();
        }

        public void Exit()
        {
            ServiceLocator.Instance.Get<UIManager>().ExitGame();
        }
    }
}