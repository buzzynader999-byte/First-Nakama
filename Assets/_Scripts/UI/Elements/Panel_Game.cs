﻿using _Scripts.Managers;
using _Scripts.Tools.Service_Locator;

namespace _Scripts.UI.Elements
{
    public class Panel_Game:Panel
    {
        public void OpenOptions()
        {
            ServiceLocator.Get<UIManager>().Close(this);
            ServiceLocator.Get<UIManager>().OpenOptionsInGame();
        }
    }
}