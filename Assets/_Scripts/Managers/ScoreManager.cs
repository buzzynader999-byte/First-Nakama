using System;
using _Scripts.Entities;
using _Scripts.Tools.Service_Locator;
using _Scripts.Weapoons;
using UnityEngine;

namespace _Scripts
{
    public class ScoreManager:IService
    {
        private NakamaConnection connection;
        public ScoreManager(NakamaConnection nakamaConnection)
        {
            connection = nakamaConnection;
            PlayerLocalNetwork.onPlayerAttacked += OnPlayerAttacked;
        }

        private void OnPlayerAttacked()
        {
            connection.SubmitScore();
        }
    }
}