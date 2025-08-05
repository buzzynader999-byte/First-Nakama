using _Scripts.Tools.Service_Locator;

namespace _Scripts.UI.Elements
{
    public class Panel_LeaderBoard:Overlay
    {
        public void CloseLeaderboard()
        {
            ServiceLocator.Instance.Get<UIManager>().Close(this);
        }
    }
}