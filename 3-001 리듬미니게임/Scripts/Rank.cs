using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Rank : MonoBehaviour
    {
        [SerializeField] TMP_Text score;        // 점수
        [SerializeField] TMP_Text number;       // 등수
        [SerializeField] GameObject[] ranks;    // 랭크 이미지(1, 2, 3)

        /// <summary>
        /// 랭크 설정
        /// </summary>
        /// <param name="_score">점수</param>
        /// <param name="_number">등수(4등, 5등일 경우 ""으로 설정)</param>
        /// <param name="rank">등수이미지(1, 2, 3등 일 경우 켜질 이미지)</param>
        public void SetRank(string _score, string _number, int rank)
        {
            score.text = _score;
            number.text = _number;

            if (rank < ranks.Length)
            {
                ranks[rank]?.gameObject.SetActive(true);
            }
        }
    }
}