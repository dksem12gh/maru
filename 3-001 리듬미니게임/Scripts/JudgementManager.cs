using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class JudgementManager : MonoBehaviour
    {
        [Header("Zone")]
        [SerializeField] BoxCollider perfectZone;   // perfect 판정 영역
        [SerializeField] BoxCollider goodZone;      // good 판정 영역

        [Header("Outline")]
        [SerializeField] Miss_event normalLine;     // 일반 아웃라인
        [SerializeField] AutoLine autoLine;         // 자동 아웃라인

        [Header("Image")]
        [SerializeField] GameObject perfectObj;     // Perfect 이미지
        [SerializeField] GameObject goodObj;        // Good 이미지
        [SerializeField] GameObject missObj;        // Miss 이미지

        [Header("Reference")]
        [SerializeField] PlayerScore playerScore;   // 각 플레이어의 점수
        Player player;
        NoteGenerator noteGenerator;

        // 판정 범위
        public Bounds judgementBound
        {
            get
            {
                return goodZone.bounds;
            }
        }

        // 게임매니저의 Init에서 호출
        public void Init(RhythmGameSettings gameSettings, NoteGenerator _noteGenerator, Player _player)
        {
            // 참조 설정
            noteGenerator = _noteGenerator;
            player = _player;

            // 판정선 설정
            bool isAuto = gameSettings.GameMode == RhythmGameMode.AutoMode ? true : false;
            autoLine.gameObject.SetActive(isAuto);
            normalLine.gameObject.SetActive(!isAuto);
            normalLine.SetNoteGenerator(noteGenerator, this);
        }

        // 노트 반정
        public void CheckNote(Note note)
        {
            // 노트의 콜리더 끄기
            note.TouchEnable(false);

            // 판정 범위 설정
            float goodMinY = goodZone.bounds.min.y;
            float goodMaxY = goodZone.bounds.max.y;
            float perfectMinY = perfectZone.bounds.min.y;
            float perfectMaxY = perfectZone.bounds.max.y;

            // 전체 판정 영역 체크
            if (note.transform.position.y >= goodMinY && note.transform.position.y <= goodMaxY)
            {
                // Perfect 처리
                if (note.transform.position.y >= perfectMinY && note.transform.position.y <= perfectMaxY)
                {
                    SetJudgementImage(true, false, false);
                    playerScore.SetScore(true);
                }
                // Good 처리
                else
                {
                    SetJudgementImage(false, true, false);
                    playerScore.SetScore(false);
                }
            }

            // 탐지한 노트를 받아와 풀 반환
            noteGenerator.ReturnToPool(note);
        }

        public void CheckAutoNote(Note note)
        {
            // 노트 콜리더 끄기
            note.TouchEnable(false);

            // 전체 라인 중 건네받은 노트의 라인에 해당하는 PianoKey의 이벤트 실행
            player.GetPianoKeys()[note.Line].AutoPressEvent();

            // 탐지한 노트를 받아와 풀 반환
            noteGenerator.ReturnToPool(note);

            // 판정 이미지 설정 및 점수 갱신
            SetJudgementImage(true, false, false);
            playerScore.SetScore(true);
        }

        // 판정 이미지 설정
        public void SetJudgementImage(bool isPerfect, bool isGood, bool isMiss)
        {
            perfectObj.SetActive(isPerfect);
            goodObj.SetActive(isGood);
            missObj.SetActive(isMiss);
        }

        // Outline 오브젝트 Collision 이벤트에 들어갈 함수
        public void MissEvent()
        {
            // Miss 이미지 및 스코어 설정
            SetJudgementImage(false, false, true);
            playerScore.MissEvent();
        }
    }
}