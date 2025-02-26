using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TouchScript.Examples.RawInput;
using DigitalMaru;
using System;

namespace Left_and_right_jumping
{
    public class GameManager : MonoBehaviour
    {
        // 시작은 컴포넌트 정리
        //프리펩 정리
        [Header("프리팹")]
        public GameObject _titlePrefab;
        public GameObject _runningManagerPrefab;

        [Space]        
        public GameObject _pausePan;
        public GameObject _root;

        [Space]
        public Camera _gameCamera;

        [Space]
        public ResultWindow _resultWindow;
        public SubSoundManager _soundManager;
        private RunningManager _runningManager;


        private Player[] _players = new Player[2];
        private int _gameTimeSec;
        private int _maxRunningCount;
        private DigitalMaru.PlayMode _playMode;

        private GameObject _guid;

        private void Awake()
        {
            MultiDisplayTouchManager multiDisplayTouchManager = FindObjectOfType<MultiDisplayTouchManager>();
            multiDisplayTouchManager.MainCam[1] = _gameCamera;
        }        

        // 게임 실행 순서 게임 시작 후 >>> 타이틀명 애니메이션 실행 >> 본 게임 >> 끝날 시 결과창 ( 무한 제외 )
        private void Start()
        {
            _playMode = _gameTimeSec == -1 ? DigitalMaru.PlayMode.Infinity : DigitalMaru.PlayMode.Normal;
            StartCoroutine(DoMainLoop(Managers.GameTime));
        }      

        private void OnDestroy()
        {
            if (_runningManager)
            {
                _runningManager.LightTouch_RemoveListener(JUMP2BTN.P0_0, light_p0_0);
                _runningManager.LightTouch_RemoveListener(JUMP2BTN.P0_1, light_p0_1);                
                _runningManager.LightTouch_RemoveListener(JUMP2BTN.P1_0, light_p1_0);
                _runningManager.LightTouch_RemoveListener(JUMP2BTN.P1_1, light_p1_1);

                Destroy(_runningManager.gameObject);
            }
            StopAllCoroutines();            
        }

        private void Update()
        {
            // 일시 정지
            if (Managers.SelGameSet.selectGameState == SelectGameState.Pause)
            {
                if (!_pausePan.activeSelf)
                {
                    _pausePan.SetActive(true);
                }
            }
            else
            {
                if (_pausePan.activeSelf)
                {
                    _pausePan.SetActive(false);
                }
            }
        }


        private void light_p0_0(Vector3 pos) { RunRunningPad(_players[0], 0); }
        private void light_p0_1(Vector3 pos) { RunRunningPad(_players[0], 1); }        
        private void light_p1_0(Vector3 pos) { RunRunningPad(_players[1], 0); }
        private void light_p1_1(Vector3 pos) { RunRunningPad(_players[1], 1); }        


        private IEnumerator DoMainLoop(int min)
        {
            yield return DoInit(min);
            yield return DoTitle();
            yield return DoRun();
            yield return DoResult();
        }

        private IEnumerator DoInit(int min)
        {
            //시간
            _gameTimeSec = min > 0 ? min * 60 : -1;

            //횟수
            _maxRunningCount = min;            

            yield return null;

            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new Player();
                _players[i].Index = i;
            }
            yield return null;

            _guid = Instantiate(_runningManagerPrefab, new Vector2(0, 0), Quaternion.identity, _root.transform);
            _runningManager = _guid.GetComponent<RunningManager>();

            _runningManager.LightTouch_AddListener(JUMP2BTN.P0_0, light_p0_0);
            _runningManager.LightTouch_AddListener(JUMP2BTN.P0_1, light_p0_1);            
            _runningManager.LightTouch_AddListener(JUMP2BTN.P1_0, light_p1_0);
            _runningManager.LightTouch_AddListener(JUMP2BTN.P1_1, light_p1_1);

            for (int i = 0; i < 2; i++)
            {
                if (_maxRunningCount == -1)
                {
                    _runningManager.GetPlayer(i).SetCountInit(0);
                }
                else
                {
                    _runningManager.GetPlayer(i).SetCountInit(_maxRunningCount);
                }
            }

            _runningManager.SetTimer(_playMode, _gameTimeSec);
            _runningManager.Hide();
        }

        private IEnumerator DoTitle()
        {
            GameObject titleGo = Instantiate(_titlePrefab);
            yield return YieldInstructionCache.WaitForSeconds(4.3f);
            Destroy(titleGo);
        }

        private IEnumerator DoRun()
        {
            // 게임 표시
            _runningManager.Show();

            // 모드별로 진행
            if (_playMode == DigitalMaru.PlayMode.Infinity)
            {
                foreach (var player in _players)
                {
                    StartCoroutine(DoRunInfinityTime(player));
                }
            }
            else
            {
                // 플레이별로 게임 시작
                foreach (var player in _players)
                {
                    StartCoroutine(DoRunLimitedTime(player));
                }
            }

            // 게임 종료 대기
            yield return YieldInstructionCache.WaitUntil(() => IsCompletedAllPlayers());
            //yield return null;
        }

        private IEnumerator DoRunLimitedTime(Player player)
        {
            // 준비
            yield return DoRunReady(player);

            // 정보 지우기
            _runningManager.GetPlayer(player.Index).ClearInfo();

            // 진행
            while (player.State == PlayerState.Run)
            {
                // 1초 경과
                yield return YieldInstructionCache.WaitForSeconds(1);

                if (_runningManager.GetPlayer(player.Index).GetCurCount() <= 0)
                {
                    player.State = PlayerState.Complete;
                }
            }

            // 종료
            player.State = PlayerState.Complete;

            // 정보 지우기
            _runningManager.GetPlayer(player.Index).ClearInfo();
        }

        private IEnumerator DoRunInfinityTime(Player player)
        {
            // 준비
            yield return DoRunReady(player);

            // 정보 지우기
            _runningManager.GetPlayer(player.Index).ClearInfo();

            // 진행
            while (player.State == PlayerState.Run)
            {
                // 1초 경과
                yield return YieldInstructionCache.WaitForSeconds(1);
            }

            // 종료
            player.State = PlayerState.Complete;

            // 정보 지우기
            _runningManager.GetPlayer(player.Index).ClearInfo();
        }


        private IEnumerator DoResult()
        {
            _guid.SetActive(false);
            _soundManager.Play(SndKind.End);
            _resultWindow.Show();            
            yield return null;
        }

        private ResultData[] GetResultDatasFromPlayers()
        {
            ResultData[] temps = new ResultData[_players.Length];
            for (int i = 0; i < _players.Length; i++)
            {
                temps[i] = _players[i].ToResultData();
            }
            return temps;
        }

        private void RunRunningPad(Player player, int padIndex)
        {
            if (player.State == PlayerState.Complete)
                return;

            _soundManager.Play(SndKind.Click);

            //왼쪽 밟음(최초시 시간줄어듬)
            //light_p방향숫자 사라짐
            //light_p반대방향숫자 생김
            //0.5증가 1번 반복하면 1증가
            //checkMidle가 true인경우에만 작동.]
            /*if (player.CheckedMiddle == true)
            {
            }*/
            //최초 시작시
            if (player.State == PlayerState.Ready)
            {
                // 상태를 Run으로 변경함.
                player.State = PlayerState.Run;
            }

            // 밟은 횟수 증가
            player.RunningCount++;

            // 완료한 사이클 횟수 표시
            var cycles = player.GetCompletedCycles();
            _runningManager.GetPlayer(player.Index).CicleCheckBtnAni(cycles.isAni);
            if (_maxRunningCount != -1)
            {
                if (cycles.isAni)
                {
                    _runningManager.GetPlayer(player.Index).SetCurCount(_runningManager.GetPlayer(player.Index).GetCurCount(),_maxRunningCount);
                }
            }
            else if (_maxRunningCount == -1)
            {
                if (cycles.isAni)
                {
                    _runningManager.GetPlayer(player.Index).SetCurCount(_runningManager.GetPlayer(player.Index).GetCurCount(), _maxRunningCount);
                }
            }

            // 현재 패드를 플레이하고 다음 패드를 준비.
            _runningManager.GetPlayer(player.Index).PlayCurrentAndReadyNextPad(padIndex);
        }

        private IEnumerator DoRunReady(Player player)
        {
            // 준비
            player.State = PlayerState.Ready;

            // 초기화
            player.RunningCount = 0;

            // 처음으로 발판을 밟을 때까지 대기
            yield return YieldInstructionCache.WaitUntil(() => player.State == PlayerState.Run);
        }

        private bool IsCompletedAllPlayers()
        {
            foreach (var player in _players)
            {
                if (player.State != PlayerState.Complete)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
