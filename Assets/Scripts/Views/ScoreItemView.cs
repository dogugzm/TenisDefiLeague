using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ScoreItemView : MonoBehaviour
    {
        public class Data
        {
            public int Score { get; }
            public Color BgColor { get; }

            public Data(int score, Color bgColor)
            {
                Score = score;
                BgColor = bgColor;
            }
        }
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image backgroundImage;
        
        public void Init(Data data)
        {
            scoreText.text = data.Score.ToString();
            backgroundImage.color = data.BgColor;
        }
        
        
    }
}