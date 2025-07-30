using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Panel_MainMenu : Panel
    {
        public void FindMatch()
        {
            ServiceLocator.Get<UIManager>().Close(this);
            GameManager.Instance.FindMatch();
        }

        public void Exit()
        {
            ServiceLocator.Get<UIManager>().ExitGame();
        }
    }
}