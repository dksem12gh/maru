using DigitalMaru.Common;
using DigitalMaru.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace DigitalMaru.Exercise.Walk
{
    public class GameManager : AGameManager
    {
        // 초기화 --------------------------------------------------
        [Header("초기화")]
        [SerializeField] LanguageManager _languageManager;
        // 콘덴츠 --------------------------------------------------
        [Header("게임")]
        [SerializeField] GameStage _gameStage;
        [Header("UI")]
        [SerializeField] UIManager _uiManager;
        [Header("시계")]
        [SerializeField] StretchingTotalTimePicture totalTime;
        [Header("타이틀, 결과, 준비")]
        [SerializeField] TitleWindow _titleWindow;
        [SerializeField] ResultWindow _resultWindow;
        [SerializeField] ReadyPlayer _readyPlayer;
        [Header("시작시 검은 오브레이")]
        [SerializeField] GameObject _blackOverlayPanel;
        [Header("사운드")]
        [SerializeField] AudioManager _audioManager;
        [SerializeField] AudioSource _sfxSuccess;


        readonly GameSettingsBridge settings = new GameSettingsBridge();

        private bool _isRunning = false;
        private IEnumerator _doPause = null;
        private List<Player> _players = new List<Player>();

        #region unity
        private void OnDestroy()
        {
            foreach (var player in _players)
            {
                player.RepCountEvent -= _uiManager.SetRepCount;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Awake()
        {
            _blackOverlayPanel.SetActive(true);
        }

        private void Start()
        {
            StartCoroutine(DoMainLoop());
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (Time.timeScale == 0)
                {
                    OnPause(false);
                }
                else
                {
                    OnPause(true);
                }
            }
#endif
        }



        #endregion


        #region 초기화
        protected override IEnumerator DoInit()
        {
            if (ApplicationSetting.IsOffline is false)
            {
                // 프로세서 초기화
                yield return new WaitUntil(() => settings.IsDone);

                // 언어
                if (string.IsNullOrWhiteSpace(settings.ContentLang) is false)
                {
                    _languageManager.ChangeLanguage(settings.ContentLang);
                    yield return new WaitWhile(() => _languageManager.IsChanging);
                }

                // 네트웍
                yield return new WaitUntil(() => NetTcpClientAsync.Instance.IsConnected);
            }

            // 사운드
            _audioManager.SetVolume(settings.SoundBgmVolume, settings.SoundSfxVolume, settings.SoundVoiceVolume);

            // 콘덴츠
            for (int i = 0; i < 1; i++)
            {
                var player = new Player
                {
                    Index = i,
                };
                player.RepCountEvent += _uiManager.SetRepCount;
                _players.Add(player);
            }

            // 초기화
            _gameStage.Init();
            totalTime.Prepare(settings);

            foreach (var player in _players)
            {
                player.SetRepCount(0);
            }
        }
        #endregion

        #region 제목
        protected override IEnumerator DoTitle()
        {
            _blackOverlayPanel.SetActive(false);
            string title = LocalizationSettings.StringDatabase.GetLocalizedString("UiTextLanguage", "Walk");
            _titleWindow.SetTitle(title);
            _titleWindow.Show();
            yield return new WaitForSeconds(0.5f);
            _titleWindow.Play();
            yield return new WaitForSeconds(1.6f);
            _titleWindow.Hide();


        }
        #endregion

        #region 진행
        protected override IEnumerator DoRun()
        {
            // 준비 ---------------
            _readyPlayer.StartBlinkReadyButtons();
            yield return _readyPlayer.DoWaitForReady(_players.Count);
            yield return _readyPlayer.DoCounterDown();
            _readyPlayer.StopBlinkReadyButtons();

            // 시작 --------------------
            // 사운드
            _audioManager.PlayBGM();

            // UI 
            _uiManager.Show();

            // 스테이지
            _gameStage.StartStage();
            //_timer.StartTimer();

            // 게임 종료 대기
            _isRunning = true;
            yield return new WaitUntil(() => { return totalTime.Sec <= 0; });
            _isRunning = false;

            // 종료 ------------
            _gameStage.StopStage();
            _uiManager.Hide();
        }

        public void CompletedRep(int playerIndex)
        {
            _players[playerIndex].IncreaseRepCount();
            _sfxSuccess.Play();
        }


        public void OnPause(bool pause)
        {
            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1f;
            }

            if (_doPause != null)
            {
                StopCoroutine(_doPause);
                _doPause = null;
            }
            _doPause = DoPause(pause);
            StartCoroutine(_doPause);
        }

        private IEnumerator DoPause(bool pause)
        {
            if (!pause && _isRunning)
            {
                yield return _readyPlayer.DoPauseAnim();
            }

            _audioManager.Pause(pause);
            _gameStage.OnPause(pause);
            _readyPlayer.Pause(pause);
            totalTime.Pause(pause);
        }
        #endregion


        #region 결과
        protected override IEnumerator DoResult()
        {
            _audioManager.StopBGM();
            yield return null;
            _resultWindow.SetUp(Mathf.FloorToInt(settings.GameTimeSecGoal), settings.PlayerCount, GetResultDatas());
            _resultWindow.Show();
            NetResult(NetCode.RESULT);

            ResultData[] GetResultDatas()
            {
                var resultDatas = new ResultData[_players.Count];
                for (int i = 0; i < resultDatas.Length; i++)
                {
                    resultDatas[i] = new ResultData()
                    {
                        CyclesCount = _players[i].RepCount,
                    };
                }
                return resultDatas;
            }
        }
        #endregion

        #region 네트웍

        public void NetRetry(string data)
        {
            var netData = new NetData().SetCode(NetCode.RETRY).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            ProcessCommandLine.SetContent(NetProcessContent.FromJson(data));
            Time.timeScale = 1.0f;
            _audioManager.UnPause();
            SceneManager.LoadScene(0);
        }

        public void NetStop(string data)
        {
            var netData = new NetData().SetCode(NetCode.STOP).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            OnPause(true);
        }

        public void NetPause(string data)
        {
            var netData = new NetData().SetCode(NetCode.PAUSE).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            OnPause(true);
        }

        public void NetResume(string data)
        {
            var netData = new NetData().SetCode(NetCode.RESUME).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            OnPause(false);
        }

        public void NetClose(string data)
        {
            var netData = new NetData().SetCode(NetCode.CLOSE).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            Application.Quit();
        }

        public void NetResult(string data)
        {
            if (ApplicationSetting.IsOffline is true)
            {
                return;
            }

            var netData = new NetData().SetCode(NetCode.RESULT).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);
        }
        #endregion
    }
}