using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;

namespace _Scripts.UI.Elements
{
    public class Popup_MatchMaking : Popup
    {
        public void CancelMatchMaking()
        {
            ServiceLocator.Get<UIManager>().Close(this);
            GameManager.Instance.CancelMatchMaking();
        }
        
    }
}
