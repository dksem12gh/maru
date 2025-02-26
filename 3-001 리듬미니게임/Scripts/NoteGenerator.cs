using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace DigitalMaru.MiniGame.MusicMovement
{
    // 노트 정보
    [Serializable]
    public class NoteInfo
    {
        // 4개의 건반 중 어디에 생성된 건지
        public int Line { get; set; }

        // Y축 좌표 값 (채보로부터 받아온다)
        public int Value { get; set; }

        public NoteInfo(int line, int value)
        {
            Line = line;
            Value = value;
        }
    }

    public class NoteGenerator : MonoBehaviour
    {
        [SerializeField] Transform noteContainer;       // 노트를 생성할 곳
        RectTransform rtViewContent;                    // noteContainer의 내부 Content RectTransform

        [Header("오디오")]
        [SerializeField] AudioManager audiomanager;
        AudioClip audioClip;
        AudioSource audioSource;

        [Header("채보")]
        [SerializeField] int musicNum;                  // 음악(채보) 번호
        [SerializeField] List<TextAsset> musicList;     // 채보 목록

        [Header("노트")]
        [SerializeField] ScrollRect scrollView;         // 노트가 생성될 ScrollRect
        Scrollbar scrollbar;                            // scrollView의 Scrollbar
        [SerializeField] Note notePrefab;               // 노트 프리팹
        [SerializeField] float playSpeed;               // 게임 속도
        float onesecondLength;                          // 오디오 클립의 1초 비율

        private List<Note> notePool = new();            // 노트 풀링 리스트

        int[] xPosition = { -225, -75, 75, 225 };       // 생성될 노트의 X 좌표값
        int[] noteCount = { 0, 0, 0, 0 };               // 각 라인별로 생성된 노트의 개수

        int maxNoteCount;       // 노트의 총량
        int noteIndex = 0;      // 노트 생성 순번

        private List<NoteInfo> noteInfos;   // 노트 정보들 (Line, Value)        

        private Coroutine _scrollNoteCoroutine;

        // 게임매니저의 init에서 호출
        public void Init(RhythmGameSettings settings)
        {
            // 게임 세팅 설정
            playSpeed = settings.GameSpeed;
            musicNum = settings.MusicNum;

            // 오디오 설정
            audioClip = audiomanager.GetCurrentClip();
            audioSource = audiomanager.GetAudioSource();

            // 스크롤 바 설정
            rtViewContent = noteContainer.GetComponent<RectTransform>();
            rtViewContent.sizeDelta = new Vector2(rtViewContent.sizeDelta.x, 100 * playSpeed * audioClip.length * 8);
            scrollbar = scrollView.verticalScrollbar;
            onesecondLength = 1f / audioClip.length;
            scrollbar.value = -settings.delayTime * onesecondLength;

            // 채보 설정
            InitializeNoteInfos();

            // 초기 노트 생성
            for (int i = 0; i < 30; i++)
            {
                CreateNote(noteInfos[i].Line, noteInfos[i].Value, i);
            }
        }

        public void Run()
        {
            // 설정 및 노트 생성 이후 음악 시작
            _scrollNoteCoroutine = StartCoroutine(nameof(ScrollNote));
        }

        // 채보 정리
        public void InitializeNoteInfos()
        {
            // 채보 정렬
            string[] musicLine = musicList[musicNum].text.Split("/");
            string[] musicline0 = musicLine[0].Split(",");
            string[] musicline1 = musicLine[1].Split(",");
            string[] musicline2 = musicLine[2].Split(",");
            string[] musicline3 = musicLine[3].Split(",");
            string[][] musicSet = new string[][] { musicline0, musicline1, musicline2, musicline3 };

            // 노트 총 개수
            maxNoteCount = musicline0.Length + musicline1.Length + musicline2.Length + musicline3.Length;

            // 노트 정보 설정
            noteInfos = new List<NoteInfo>();
            for (int line = 0; line < musicSet.Length; line++)
            {
                for (int i = 0; i < musicSet[line].Length; i++)
                {
                    int value = int.Parse(musicSet[line][i]);
                    noteInfos.Add(new NoteInfo(line, value));
                }
            }

            // 값이 낮은 순서로 정렬.
            noteInfos.Sort((a, b) => b.Value.CompareTo(a.Value));
            noteInfos.Reverse();
        }

        /// <summary>
        /// 노트 생성
        /// </summary>
        /// <param name="line">노트의 라인 위치(0 ~ 3)</param>
        /// <param name="value">노트 Y포지션 값</param>
        /// <param name="order">생성 순서</param>
        private void CreateNote(int line, int value, int order)
        {
            Note note = Instantiate(notePrefab, noteContainer);
            note.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition[line], value * playSpeed * 100);
            note.Line = line;
            note.sequentialOrder = order;

            notePool.Add(note);
            noteCount[line]++;
            noteIndex++;
        }

        private void OnDestroy()
        {
            // 풀링 초기화
            for(int i = 0; i < notePool.Count; i++)
            {
                Destroy(notePool[i]);
            }
            notePool.Clear();
        }

        // 일시 정지 이벤트 (노트 스크롤)
        public void Pause(bool pause)
        {            
            if (pause)
            {
                StopCoroutine(_scrollNoteCoroutine);
            }
            else
            {
                if(_scrollNoteCoroutine != null)
                    StopCoroutine(_scrollNoteCoroutine);

                _scrollNoteCoroutine = StartCoroutine(nameof(ScrollNote));
            }
        }

        public IEnumerator ScrollNote()
        {            
            float starttime = Time.time;
            while (Managers.SelGameSet.selectGameState == SelectGameState.Play)
            {                
                scrollbar.value += onesecondLength * (Time.time - starttime);
                starttime = Time.time;
                yield return null;
            }
        }

        // 판정을 통해 해당 노트를 재사용
        public void ReturnToPool(Note note)
        {
            // 이미 최대 생성이면 중지
            if (noteIndex == maxNoteCount) return;

            // 노트 순번에 해당하는 정보를 받아온다.
            NoteInfo noteInfo = noteInfos[noteIndex];

            // 위치 및 속성 설정
            note.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition[noteInfo.Line], noteInfo.Value * playSpeed * 100);
            note.Line = noteInfo.Line;
            note.sequentialOrder = noteIndex;

            // 터치 허용
            note.TouchEnable(true);

            // 리스트에 저장은 하나 실사용은 X → 이후에 제거 or 사용
            notePool.Add(note);

            // 노트 순번 갱신
            noteIndex++;
        }
    }
}