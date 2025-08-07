using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.LeaderBoard
{
    public class LeaderBoardRecord : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Image userRankImage;
        [SerializeField] Image bgColor;

        public void SetUp(string score, string username,string rank, Sprite rankSprite)
        {
            scoreText.text = score;
            usernameText.text = username;
            userRankImage.sprite = rankSprite;
            rankText.text = rank;
        }

        public void SetColor(Color color)
        {
            bgColor.color = color;
        }

        public void SetName(string userName)
        {
            usernameText.text = userName;
        }
    }
}