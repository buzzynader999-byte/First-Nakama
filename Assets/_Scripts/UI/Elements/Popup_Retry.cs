using System;
using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Popup_Retry : Popup
    {
        private NetworkManager Networdk=>NetworkManager.Instance;

        public async void RetryConnection()
        {
            UIManager.Instance.Close(this);
            await Networdk.TryConnect();
        }
    }
}