using DigitalMaru.Network;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class RhythmGameSettings
    {
        // 게임속도
        public RhythmGameSpeed GameSpeedType => Managers.SelGameSet.rhythmSpeed;
        public float GameSpeed
        {
            get
            {
                return GameSpeedType switch
                {
                    RhythmGameSpeed.level01 => 0.5f,
                    RhythmGameSpeed.level02 => 0.75f,
                    RhythmGameSpeed.level03 => 1,
                    _ => 1,
                };
            }
        }

        // 게임 모드
        public RhythmGameMode GameMode => Managers.SelGameSet.rhythmMode;
        public int PlayMode
        {
            get
            {
                return GameMode switch
                {
                    RhythmGameMode.AutoMode => 0,
                    RhythmGameMode.MovementMode => 1,
                    RhythmGameMode.FreeMode => 2,
                    _ => 0,
                };
            }
        }

        // 모드 이름
        public string ModeName
        {
            get
            {
                return GameMode switch
                {
                    RhythmGameMode.AutoMode => "AutoMode",
                    RhythmGameMode.MovementMode => "Movement",
                    RhythmGameMode.FreeMode => "FreeMode",
                    _ => "",
                };
            }
        }

        // 곡 이름
        public string MusicName
        {
            get
            {
                if (GameMode != RhythmGameMode.FreeMode)
                    return Managers.SelGameSet.selectMusic._musicName;

                return "";
            }
        }

        // 작곡가 이름
        public string ArtistName
        {
            get
            {
                if (GameMode != RhythmGameMode.FreeMode)
                    return Managers.SelGameSet.selectMusic._atist;

                return "";
            }
        }

        // 음악 인덱스
        public int MusicNum
        {
            get
            {
                if (GameMode != RhythmGameMode.FreeMode)
                    return Managers.SelGameSet.selectMusic._musicNum;
                return 0;
            }
        }

        // 기존에 불러오는 노트들의 위치가 판정선과 너무 가까움
        // → 일정 수치만큼 노트의 위치를 조정, 그에 따른 음악 재생 시간 딜레이가 발생해야함.
        // → 이 수치만큼의 딜레이.
        public float delayTime
        {
            get { return 3f; }
        }

    }

    public class GameManager : MultiDisplayTouchGameManager
    {
        readonly RhythmGameSettings settings = new RhythmGameSettings();

        [SerializeField] GameObject _PausePan;          // 일시정지
        [Header("Canvas")]
        public Canvas freeCanvas;           // 자유연주 캔버스
        public Canvas normalCanvas;         // 일반 캔버스
        Canvas activeCanvas;                // 게임 모드에 따라 설정할 추적용 캔버스 

        [Header("노래 세팅")]
        private int _playMode = 1; //0자동연주, 1 수동연주, 2자유연주

        [Header("Managers")]
        [SerializeField] ResultManager resultManager;   // 결과 매니저
        [SerializeField] AudioManager audioManager;     // 음향 매니저
        [SerializeField] Player[] players;              // 각 플레이어
        [SerializeField] Title title;                   // 시작 타이틀
        [SerializeField] MusicElement _musicElement;

        [Header("Network")]
        [SerializeField] private ProcessCommandLine _processCommandLine;
        [SerializeField] private LanguageManager _languageManager;


        bool isFreeMode;
        bool _isPlaying = false;
        bool _isCountDown = false;
        Coroutine _pause_Coroutine;        

        protected override IEnumerator DoInit()
        {
            // 프로세서 초기화
            yield return new WaitUntil(() => _processCommandLine.IsDone);

            // 언어
            if (string.IsNullOrWhiteSpace(_processCommandLine.Content.contentLang) is false)
            {
                _languageManager.ChangeLanguage(_processCommandLine.Content.contentLang);
                yield return new WaitWhile(() => _languageManager.IsChanging);
            }

            // 네트웍
            yield return new WaitUntil(() => NetTcpClientAsync.Instance.IsConnected);            

            //리듬겜용 연결
            Managers.SelGameSet.rhythmMode = (RhythmGameMode)_processCommandLine.Content.rhythmMode;

            if (settings.GameMode != RhythmGameMode.FreeMode)
            {
                // 사운드                
                audioManager.SetBgmVolum(_processCommandLine.Content.soundBgmVolume);
                audioManager.SetSfxVolume(_processCommandLine.Content.soundSfxVolume);

                _musicElement.SelectMusic(_processCommandLine.Content.rhythmId);
                Managers.SelGameSet.rhythmSpeed = (RhythmGameSpeed)_processCommandLine.Content.rhythmLevel;
                Managers.SelGameSet.playerCount = (int)_processCommandLine.Content.contentPlayerMax;   
            }
            else //자유모드용 사운드 동기화
            {
                audioManager.SetVolum_FreeMode(_processCommandLine.Content.soundSfxVolume);
            }
            //보이스 연동
            audioManager.SetVoiceVolume(_processCommandLine.Content.soundVoiceVolume);


            yield return null;

            _playMode = settings.PlayMode;

            isFreeMode = (_playMode == 2);
            Managers.SelGameSet.selectGameState = SelectGameState.None;

            // 캔버스 설정
            freeCanvas.gameObject.SetActive(isFreeMode);
            normalCanvas.gameObject.SetActive(!isFreeMode);
            activeCanvas = isFreeMode ? freeCanvas : normalCanvas;

            //1인용용 간단한 설정
            if (Managers.SelGameSet.playerCount == 1)
            {
                //카운트다운 위치가 판정이랑 겹쳐서 y축으로 +200만큼이동
                audioManager.OnePlayerCountSet();
                //플레이어1위치를 중앙으로
                players[0].transform.localPosition = Vector3.zero;
                players[1].gameObject.SetActive(false);
                //플레이어2를 배열에서 빼버리면 자연스럽게 1인용으로 실행됨.
                players = new Player[1] { players[0] };
                //1인용전용 결과창이 필요할수도?                
            }

            // 자유 모드가 아니면
            if (!isFreeMode)
            {
                // 각종 매니저 및 플레이어 세팅
                audioManager.Init(settings);
                resultManager.Init(settings, players);                
                foreach (Player player in players) { player.Init(settings); }
            }

            // 타이틀
            title.gameObject.SetActive(true);
            title.Init(settings);

            yield break;
        }
        protected override IEnumerator DoResult()
        {            
            // 자유 연주면 결과창 X
            if (isFreeMode) yield break;

            NetResult("");

            // 현재 켜져 있는 게임창(Active Canvas)을 끄고 결과창 연출 시작
            activeCanvas.gameObject.SetActive(false);
            resultManager.ShowResult();
        }

        protected override IEnumerator DoRun()
        {
            // 자유 연주면 플레이 세팅 X
            if (isFreeMode) yield break;

            // 음성 시작 미리 로드해놓고 멈춰놔야지 노래시작할때 작은 렉이나 딜레이가 없다.
            audioManager.GetAudioSource().Play();
            audioManager.GetAudioSource().Pause();

            yield return new WaitForSeconds(1);

            // 3초 카운트            
            yield return audioManager.ThreeCount();            

            Managers.SelGameSet.selectGameState = SelectGameState.Play;
            _isPlaying = true;

            // 각 플레이어 시작
            foreach (Player player in players) { player.Run(); }
            yield return new WaitForSeconds(settings.delayTime);

            audioManager.GetAudioSource().UnPause();

            // 게임 상태가 Finish가 될 떄 까지 대기, 이후 DoResult로 넘어감
            yield return new WaitUntil(() => Managers.SelGameSet.selectGameState == SelectGameState.Finish);
        }
        // 타이틀 창 1.5초 후 끄기
        protected override IEnumerator DoTitle()
        {
            yield return new WaitForSeconds(1.5f);

            title.gameObject.SetActive(false);
        }

        // 일시정지
        protected override void OnPause(bool pause)
        {            
            if (_pause_Coroutine != null)
            {
                StopCoroutine(_pause_Coroutine);
            }

            _pause_Coroutine = StartCoroutine(PauseEvent(pause));

        }

        IEnumerator PauseEvent(bool pause)
        {            
            
            // 오디오 매니저로부터 메인 오디오 소스 받아오기
            AudioSource audioSource = audioManager.GetAudioSource();

            // 일시정지
            if (pause)
            {
                // 일시정지 창 활성화 및 오디오 일시정지
                _PausePan.SetActive(pause);
                audioSource.Pause();

                // 플레이어 일시정지
                foreach (Player player in players) { player.Pause(pause); }
            }
            // 일시정지 해제
            else
            {
                // 일시정지 창 비활성화
                _PausePan.SetActive(pause);

                // 3초 대기                
                yield return audioManager.ThreeCount();
                
                // 플레이어, 오디오 일시정지 해제
                audioSource.UnPause();
                // 게임 상태 플레이로 갱신
                Managers.SelGameSet.selectGameState = SelectGameState.Play;

                foreach (Player player in players) { player.Pause(pause); }

            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                NetPause("");
            else if (Input.GetKeyDown(KeyCode.L))
                NetResume("");
        }

        //실행된상태면 무시하는 코드 추가?
        #region 네트워크 이벤트 호출
        public void NetPause(string data)
        {
            var netData = new NetData().SetCode(NetCode.PAUSE).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);
            if(_isPlaying) OnPause(true);
            else Time.timeScale = 0;
        }

        public void NetResume(string data)
        {
            var netData = new NetData().SetCode(NetCode.RESUME).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);
            if (_isPlaying) OnPause(false);
            else Time.timeScale = 1;
        }

        public void NetClose(string data)
        {
            StartCoroutine(NetCloseCoroutine(data));
        }

        private IEnumerator NetCloseCoroutine(string data)
        {
            var netData = new NetData().SetCode(NetCode.CLOSE).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            // 0.5초 동안 대기하여 데이터 전송 완료를 대략적으로 보장
            yield return new WaitForSeconds(0.5f);

            // 전송 완료 후 연결 닫기 및 애플리케이션 종료
            NetTcpClientAsync.Instance.Close();
            Application.Quit();
        }


        public void NetRetry(string data)
        {
            var netData = new NetData().SetCode(NetCode.RETRY).SetData("echo");
            NetTcpClientAsync.Instance.Send(netData);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        private void NetResult(string data)
        {
            var netData = new NetData().SetCode(NetCode.RESULT).SetData(data);
            NetTcpClientAsync.Instance.Send(netData);            
        }        
        #endregion
    }
}