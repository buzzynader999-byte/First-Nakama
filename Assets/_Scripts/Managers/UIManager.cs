using _Scripts.Tools.Service_Locator;
using _Scripts.UI.Elements;

namespace _Scripts.Managers
{
    public class UIManager : MonoService
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

        public Overlay OpenFindMatch()
        {
            return _client.OpenOverlay<Popup_MatchMaking>(false, true);
        }

        public Overlay OpenLeaderBoard()
        {
            return _client.OpenOverlay<Panel_LeaderBoard>(false, true);
        }

        public void ExitGame()
        {
            ScoreManager sM = Services.Get<ScoreManager>();
            sM.SubmitScores();
            print("Added to scores in server | current score : " + sM.CurrentScore + " | Score in Server: " +
                  sM.ScoreInServer);
        }

        public Overlay OpenMainMenu()
        {
            _client.Clear();
            return _client.OpenOverlay<Panel_MainMenu>();
        }

        public Overlay OpenGameMenu()
        {
            _client.Clear();
            return _client.OpenOverlay<Panel_Game>();
        }

        public Overlay OpenOptionsInGame()
        {
            return _client.OpenOverlay<Popup_Options>();
        }

        public void Close(Overlay target)
        {
            _client.CloseOverlay(target);
        }

        public Overlay OpenRetryConnection()
        {
            print("Opening retry popup");
            return _client.OpenOverlay<Popup_Retry>(false, true);
        }

        public Overlay OpenProfile()
        {
            return _client.OpenOverlay<Panel_Profile>(false, true);
        }

        public Overlay OpenRenamingPopup()
        {
            return _client.OpenOverlay<Popup_Renaming>(false, true);
        }

        public Overlay OpenConnectingPopup()
        {
            return _client.OpenOverlay<Popup_Connecting>(false, true);
        }

        protected override void Register()
        {
            Services.Register(this);
        }

        protected override void UnRegister()
        {
            Services.Unregister<UIManager>();
        }
    }
}