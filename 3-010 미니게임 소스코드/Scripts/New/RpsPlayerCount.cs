using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsPlayerCount : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;

        public void ScoreUpdate(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}