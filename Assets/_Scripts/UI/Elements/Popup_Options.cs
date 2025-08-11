    using System;
    using _Scripts.Managers;
    using _Scripts.Tools.Service_Locator;
    using UnityEngine;

    namespace _Scripts.UI.Elements
    {
        public class Popup_Options:Popup
        {
            public void LeaveGameNow()
            {
                GameManager.Instance.LeaveMatch();
            }

            public void CloseOptions()
            {
                Services.Get<UIManager>().Close(this);
                Services.Get<UIManager>().OpenGameMenu();
            }
        }
    }
