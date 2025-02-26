using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class PlayerScore : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] TMP_Text comboText;        // 콤보
        [SerializeField] TMP_Text scoreText;        // 점수

        [SerializeField] AudioSource audioSource;   // 메인 오디오 소스
        [SerializeField] Animator uiAnimator;

        // 스코어 정보
        int score;
        int combo;
        public int maxCombo { get; private set; }

        // 판정 개수들
        public int perfectCount { get; private set; }
        public int goodCount { get; private set; }
        public int missCount { get; private set; }

        public int Score
        {
            get => score;
            set
            {
                // 스코어 설정 시 UI텍스트 갱신
                score = value;
                scoreText.text = score.ToString();
            }
        }

        public int Combo
        {
            get
            {
                return combo;
            }
            set
            {
                // 콤보 설정 시 UI텍스트 및 최대 콤보갱신
                SetMaxCombo();

                combo = value;
                comboText.text = combo.ToString();
            }
        }

        // 점수 갱신
        public void SetScore(bool isPerfect)
        {
            Combo++;
            SetEffect();

            // 퍼펙트
            if (isPerfect)
            {
                perfectCount++;
                Score += 100 + Combo;
            }
            // 굿
            else
            {
                goodCount++;
                Score += 50 + Combo;
            }
        }

        public void MissEvent()
        {
            missCount++;
            Combo = 0;
        }

        void SetEffect()
        {
            //소리 및 파티클 재생
            if (Combo % 50 == 0)
            {
                audioSource.Play();
                uiAnimator.SetTrigger("spacil");
            }
            else
                uiAnimator.SetTrigger("nomal");
        }

        // NoteGenerator의 EndMusicEvent에서 호출
        public void SetMaxCombo()
        {
            if (Combo > maxCombo)
                maxCombo = Combo;
        }


    }
}