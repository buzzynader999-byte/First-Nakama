using System;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Panel_Profile : Overlay
    {
        [SerializeField] private TextMeshProUGUI userDisplayName;

        [SerializeField] private TextMeshProUGUI userName;

        private NakamaConnection Connection=>NetworkManager.Instance.Connection;

        private void OnEnable()
        {
            Popup_Renaming.onDisplayNameChanged += OnDisplayNameChanged;
            OnDisplayNameChanged();
        }

        private void OnDisable()
        {
            Popup_Renaming.onDisplayNameChanged -= OnDisplayNameChanged;
        }

        private async void OnDisplayNameChanged()
        {
            var account = await Connection.Client.GetAccountAsync(Connection.Session);
            SetDisplayName(account.User.DisplayName);
            SetUsername(account.User.Username);
        }

        void SetDisplayName(string userName)
        {
            userDisplayName.text = String.IsNullOrEmpty(userName) ? ("No Display Name") : userName;
        }

        void SetUsername(string id)
        {
            userName.text = String.IsNullOrEmpty(id) ? ("No User Name") : id;
        }

        public void OpenRenamingPopup()
        {
            UIManager.Instance.OpenRenamingPopup();
        }
    }
}