using System;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.Elements
{
    public class Popup_Renaming:Overlay
    {
        private NakamaConnection _connection => GameManager.Instance.NakamaConnection;
        [SerializeField] private TMP_InputField userNameInput;
        public static Action onDisplayNameChanged;
        public void RenameUser()
        {
            if(String.IsNullOrEmpty(userNameInput.text))return;
            Debug.Log(userNameInput.text);
            GameManager.Instance.NakamaConnection.Client.UpdateAccountAsync(_connection.Session, _connection.UserName,
                userNameInput.text);
            onDisplayNameChanged?.Invoke();
        }
    }
}