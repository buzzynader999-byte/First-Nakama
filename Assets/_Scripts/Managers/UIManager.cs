using _Scripts.Tools.Service_Locator;
using _Scripts.UI.Elements;

namespace _Scripts.Managers
{
    public class UIManager : Service
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

        public void OpenLeaderBoard()
        {
            _client.OpenOverlay<Panel_LeaderBoard>(false, true);
        }

        public void ExitGame()
        {
            ScoreManager sM = ServiceLocator.Instance.Get<ScoreManager>();
            sM.SubmitScores();
            print("Added to scores in server | current score : " + sM.CurrentScore + " | Score in Server: " +
                  sM.ScoreInServer);
        }

        public void OpenMainMenu()
        {
            _client.Clear();
            _client.OpenOverlay<Panel_MainMenu>();
        }

        public void OpenGameMenu()
        {
            _client.Clear();
            _client.OpenOverlay<Panel_Game>();
        }

        public void OpenOptionsInGame()
        {
            _client.OpenOverlay<Popup_Options>();
        }

        public void Close(Overlay target)
        {
            _client.CloseOverlay(target);
        }

        public void OpenRetryConnection()
        {
            _client.OpenOverlay<Popup_Retry>(false, true);
        }

        public void OpenProfile()
        {
            _client.OpenOverlay<Panel_Profile>(false, true);
        }

        public void OpenRenamingPopup()
        {
            _client.OpenOverlay<Popup_Renaming>(false, true);
        }
    }
}