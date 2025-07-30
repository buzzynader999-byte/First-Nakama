    using System;
    using _Scripts.Managers;
    using UnityEngine;

    namespace _Scripts.UI.Elements
    {
        public class Popup_Options:Popup
        {
            public void LeaveGameNow()
            {
                GameManager.Instance.Leavematch();
            }

            public void CloseOptions()
            {
                UIManager.Instance.Close(this);
                UIManager.Instance.OpenGameMenu();
            }
        }
    }
