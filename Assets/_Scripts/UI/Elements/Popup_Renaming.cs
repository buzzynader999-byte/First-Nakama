using System;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Popup_Renaming : Overlay
    {
        private NakamaConnection Connection => NetworkManager.Instance.Connection;
        [SerializeField] private TMP_InputField userNameInput;
        public static Action onDisplayNameChanged;


        public void RenameUser()
        {
            if (String.IsNullOrEmpty(userNameInput.text)) return;
            Debug.Log(userNameInput.text);
            Connection.Client.UpdateAccountAsync(Connection.Session, Connection.UserName,
                userNameInput.text);
            onDisplayNameChanged?.Invoke();
        }
    }
}