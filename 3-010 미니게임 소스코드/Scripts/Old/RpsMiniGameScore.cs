using TMPro;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMiniGameScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        int score;

        public int Score { get =>score;}

        public virtual void ScoreUp()
        {
            SetScore(score + 1);
        }

        void SetScore (int value)
        {
            score = value;
            scoreText.text = score.ToString();
        }

        public int ToResultData()
        {
            return score;
        }
    }
}
