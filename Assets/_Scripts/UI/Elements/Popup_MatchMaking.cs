using _Scripts.Managers;

namespace _Scripts.UI.Elements
{
    public class Popup_MatchMaking : Popup
    {
        public void CancelMatchMaking()
        {
            UIManager.Instance.Close(this);
            GameManager.Instance.CancelMatchMaking();
        }
        
    }
}
