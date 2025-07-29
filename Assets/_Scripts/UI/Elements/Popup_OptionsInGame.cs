    using _Scripts.Managers;

    namespace _Scripts.UI.Elements
    {
        public class Popup_OptionsInGame:Popup
        {
            public void LeaveGameNow()
            {
                GameManager.Instance.Leavematch();
            }
        }
    }
