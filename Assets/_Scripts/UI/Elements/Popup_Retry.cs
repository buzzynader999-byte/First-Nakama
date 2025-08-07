using _Scripts.Managers;

namespace _Scripts.UI.Elements
{
    public class Popup_Retry:Popup
    {
        public void RetryConnection()
        {
            UIManager.Instance.Close(this);
            GameManager.Instance.TryConnect();
        }
    }
}