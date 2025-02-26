using DigitalMaru.Network;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsGameManager : MultiDisplayTouchGameManager
    {
        // 초기화 --------------------------------------------------
        [Header("초기화")]
        [SerializeField] ProcessCommandLine _processCommandLine;
        [SerializeField] LanguageManager _languageManager;
        [Space]
        [SerializeField] GameObject _multiTouchObj;
        [SerializeField] RpsGamePlayManager gamePlayManager;
        [SerializeField] TitleWindowFactory title;
        [Space]
        [SerializeField] BgmPlayerManager _randomBgm;
        [SerializeField] GameObject pauseObj;
        [SerializeField] GameObject resultWindow;
        [Space]
        [SerializeField] GameObject blackOverlayPanel;

        protected override void Awake()
        {
            base.Awake();
            blackOverlayPanel.SetActive(true);
            pauseObj.SetActive(true);
        }

        protected override IEnumerator DoInit()
        {
            if (ApplicationSetting.IsOffline is false)
            {
                // 프로세서 초기화
                yield return new WaitUntil(() => ProcessCommandLine.IsDone);

                // 언어
                if (string.IsNullOrWhiteSpace(ProcessCommandLine.ContentLang) is false)
                {
                    _languageManager.ChangeLanguage(ProcessCommandLine.ContentLang);
                    yield return new WaitWhile(() => _languageManager.IsChanging);
                }

                // 네트웍
                yield return new WaitUntil(() => NetTcpClientAsync.Instance.IsConnected);
            }


            _randomBgm.SetBgmVolume(ProcessCommandLine.SoundBgmVolume);
            _randomBgm.SetSfxVolume(ProcessCommandLine.SoundSfxVolume);

            // 콘텐츠
            gamePlayManager.Prepare();
            yield return null;
        }

        protected override IEnumerator DoResult()
        {
            _randomBgm.ResultStopBgm();
            var wnd = Instantiate(resultWindow, transform).GetComponent<MiniGameResultWindow>();
            wnd.Show(gamePlayManager.GetResult(), ProcessCommandLine.SoundSfxVolume);


            NetResult(NetCode.RESULT);
            _multiTouchObj.SetActive(false);

            yield return null;
        }

        protected override IEnumerator DoRun()
        {
            yield return gamePlayManager.Run();
        }

        protected override IEnumerator DoTitle()
        {
            blackOverlayPanel.SetActive(false);
            yield return title.CreateAndRemoveWithDelayed();
            pauseObj.SetActive(false);
            _randomBgm.RandomSelectPlay();
        }

        protected override void OnPause(bool pause)
        {
            gamePlayManager.OnPause(pause);
        }

        #region 네트웍
        public void NetAuth(string data)
        {

        }

        public void NetStop(string data)
        {
            var netData = new NetData().SetCode(NetCode.STOP).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            OnPause(true);
            _randomBgm.PauseBgm();
            Time.timeScale = 0;
        }

        public void NetRetry(string data)
        {
            var netData = new NetData().SetCode(NetCode.RETRY).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);

            ProcessCommandLine.SetContent(NetProcessContent.FromJson(data));

            Time.timeScale = 1;
            _randomBgm.UnPauseBgm();
            SceneManager.LoadScene(0);
        }

        public void NetResult(string data)
        {
            if (ApplicationSetting.IsOffline is true)
            {
                return;
            }

            var netData = new NetData().SetCode(NetCode.RESULT).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);
        }

        public void NetPause(string data)
        {
            var netData = new NetData().SetCode(NetCode.PAUSE).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);
            OnPause(true);
            pauseObj.SetActive(true);
            _randomBgm.PauseBgm();
        }

        public void NetResume(string data)
        {
            var netData = new NetData().SetCode(NetCode.RESUME).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);

            Time.timeScale = 1;

            OnPause(false);
            pauseObj.SetActive(false);
            _randomBgm.UnPauseBgm();
        }

        public void NetClose(string data)
        {
            var netData = new NetData().SetCode(NetCode.CLOSE).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);

            Application.Quit();
        }
        #endregion
    }
}