namespace _Scripts.UI.Elements
{
    public class Panel_Game:Panel
    {
        public void OpenOptions()
        {
            UIManager.Instance.Close(this);
            UIManager.Instance.OpenOptionsInGame();
        }
    }
}