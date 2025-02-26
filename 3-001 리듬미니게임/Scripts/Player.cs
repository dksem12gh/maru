using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Player : MonoBehaviour
    {
        [SerializeField] PianoKey[] pianoKeys;                  // 건반 키
        [SerializeField] JudgementManager judgementManager;     // 판정 매니저
        [SerializeField] NoteGenerator noteGenerator;           // 노트 생성기

        PlayerScore myScore;        // 각종 스코어 저장

        // 게임매니저 init에서 호출
        public void Init(RhythmGameSettings gameSettings)
        {
            judgementManager.Init(gameSettings, noteGenerator, this);
            noteGenerator.Init(gameSettings);

            foreach (PianoKey key in pianoKeys)
            {
                key.Init(judgementManager);
                // 자동 연주 모드면, 건반 키의 터치 미허용
                if (gameSettings.GameMode == RhythmGameMode.AutoMode)
                {
                    key.GetComponent<BoxCollider>().enabled = false;
                }
            }

            myScore = GetComponent<PlayerScore>();
        }

        // 노트 생성 시작
        public void Run()
        {
            noteGenerator.Run();
        }

        // 노트 일시정지
        public void Pause(bool pause)
        {
            noteGenerator.Pause(pause);
            foreach(PianoKey k in pianoKeys)
            {
                k.TouchEnable(!pause);
            }
        }

        public PlayerScore GetPlayerScore()
        {
            return myScore;
        }

        public PianoKey[] GetPianoKeys()
        {
            return pianoKeys;
        }
    }
}