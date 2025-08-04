using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class LeaderBoardRecord : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Image userRankImage;

        public void SetUp(string score, string username,string rank, Sprite rankSprite)
        {
            scoreText.text = score;
            usernameText.text = username;
            userRankImage.sprite = rankSprite;
            rankText.text = rank;
        }
    }
}