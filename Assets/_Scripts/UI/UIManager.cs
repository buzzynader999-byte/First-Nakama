using System;
using _Scripts.Managers;
using _Scripts.UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { set; get; }
        ClientCoordinator _client => ClientCoordinator.Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OpenFindMatch()
        {
            
            _client.OpenOverlay<Popup_MatchMaking>(false, true);
        }

        public void ExitGame()
        {
            _client.OpenOverlay<Popup_Exit>();
        }

        public void OpenSettings()
        {
            _client.OpenOverlay<Panel_Settings>();
        }
        
        public void OpenMainMenu()
        {
            //_client.CloseAllOverlays();
            _client.OpenOverlay<Panel_MainMenu>();
        }

        public void OpenGameMenu()
        {
            _client.CloseOverlay(_client.overlayList[0]);
            _client.OpenOverlay<Panel_Game>();
        }

        public void OpenOptionsInGame()
        {
            _client.OpenOverlay<Popup_OptionsInGame>();
        }

        public void Close(Overlay target)
        {
            _client.CloseOverlay(target);
        }
    }
}