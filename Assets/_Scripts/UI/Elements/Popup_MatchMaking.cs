namespace _Scripts.UI.Elements
{
    public class Popup_MatchMaking : Popup
    {
        public void CancelMatchMaking()
        {
            UIManager.Instance.OpenMainMenu();
            UIManager.Instance.Close(this);
        }
        
    }
}
