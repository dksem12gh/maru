using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TouchScript.Examples.RawInput;
using DigitalMaru;

namespace Left_and_right_jumping
{
    public class GameManagerSide : MonoBehaviour
    {
        // 시작은 컴포넌트 정리

        //프리펩 정리
        [Header("컴포넌트")]
        public GameObject pTitle;                     //타이틀 받아오기
        public GameObject pResult;                      //결과창 받아오기
        public GameObject pGuide;                     //가이드 << 실제 인게임 버튼 등 UI 구성품 가져오기
        public Transform _Sound;                      //사운드 << 사운드 컴포넌트 가져오기
        public GameObject _PausePan;                 //일시정지 패널 가져오기
        public Camera GameCamera;

        // 스프라이트 변경 컴포넌트
        [Header("스프라이트 변경 건들 X")]
        public Sprite[] nSprite;                     // 노말 스프라이트

        //전역 변수
        private int   _SetPlay = 0;                                                               //게임 단계를 검사
        private int[] _step = {-1,-1};                                                            //게임 단계 (무제한) 계산
        private int[] _resultCount = { 0 , 0 };                                                   // 결과 값에 들어갈 총 카운트 
        private float[,] _lasttime = new float[2,2] { {0,0},{0,0} };                // 시간 계산 
        private bool[] _stepcheck = {false,false};
        private int[] _returnStep = {0,0};                                                        // 초기화 시켜주기 스텝

        //프레스 체크를 위한 함수
        private List<Transform>[] TouchList = new List<Transform>[2];                  // 터치리스트
        private List<Transform>[] spriteList = new List<Transform>[2];                 // 노말 스프라이트
        private List<Transform>[] particleList = new List<Transform>[2];                 // 파티클 리스트
        private int[,] _press = new int[2,2] {{0,0},{0,0}};         // 2명의 플레이어, 각각 터치 전용 4개의 버튼

        private bool[,] _isCoroutineRunningReleaseCheck;
        private bool[] _isCoroutineRunningStep;

        AudioSource soundEnd;
        AudioSource soundClick;
        AudioSource soundSuccess;

        GameObject[] guideUiTxtPlayer = new GameObject[2];
        TMP_Text[] guideUiCountTxt = new TMP_Text[2];
        Animation[] guidUiCountPlayer = new Animation[2];

        SpriteRenderer[,] spritePlayerSpriteRender = new SpriteRenderer[2, 2];
        Animator[,] spriteListAni = new Animator[2, 2]; 

        GameObject guidTouch1Pause;
        GameObject guidTouch2Pause;

        TMP_Text resultCount1;
        TMP_Text resultCount2;

        Animator resultAni;
        private void Awake()
        {
            MultiDisplayTouchManager multiDisplayTouchManager = FindObjectOfType<MultiDisplayTouchManager>();
            multiDisplayTouchManager.MainCam[1] = GameCamera;
        }        

        // 게임 실행 순서 게임 시작 후 >>> 타이틀명 애니메이션 실행 >> 본 게임 >> 끝날 시 결과창 ( 무한 제외 )
        private void Start()
        {
            Changecount(Managers.GameTime);      // 게임 시작시 메인 키오스크에서 선택한 게임 타임 ( 시간 일지, 횟수 일지 수치 정해달라고 하라고 함 )
        }

        private void OnDestroy()
        {
            for (int i = 0; i < 2; i++)
            {
                if (TouchList[i] == null) break;

                TouchList[i].Clear();
                spriteList[i].Clear();
                particleList[i].Clear();
            }
            StopAllCoroutines();
        }

        private void Changecount(int num)       // 기획서에는 ++로 증가 시켜서 총 횟수 값에 가는데 라쿤 님이 이때가지 --로 만들어서 나도 --로 통일 시킴.
        {
            _SetPlay = num;      // 총 세트 수 저장을 위함
            if (num != -1)       //무제한이 아닐때만 메인 키오스크에서 num값을 받아와서 사용
            {
                _resultCount[0] = num;
                _resultCount[1] = num;
            }
            else                 //무제한이 라면 -1로 받아서 사용
            {
                _resultCount[0] = -1;
                _resultCount[1] = -1;
            }

            //타이틀->준비->시작 표시
            StartCoroutine(Title());
        }

        private IEnumerator Title()
        {
            pTitle = Instantiate(pTitle, GameCamera.transform);      // 타이틀 instantiate로 프리펩 소환 
            yield return YieldInstructionCache.WaitForSeconds(4.5f);                     // 4초 뒤 파괴 실행
            Destroy(pTitle);

            Game_Play();                                            // 게임 실행
        }
        private IEnumerator Stop()
        {
            while (true)
            {
                while (Managers.SelGameSet.selectGameState == SelectGameState.Pause)
                {
                    _PausePan.gameObject.SetActive(true);
                    yield return YieldInstructionCache.WaitForSeconds(0.1f);
                }
                _PausePan.gameObject.SetActive(false);
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// 자세를 취하세요 등 게임 시작 시 필요한 설정
        /// 플레이어의 설정은 버튼의 num으로 결정이 되므로 총 결과 값을 매니저의 enum으로 넣을 거면 _SetPlay를 이용하여 끝날 때 넣으면 될 것 같음.
        /// </summary>
        private void Game_Play()
        {
            pGuide.SetActive(true);         //가이드 키기

            if (_resultCount[0] <= 0 && _resultCount[1] <= 0) // 무제한이면! << -1 값으로 받아왔기 때문에 ++횟수를 위해 0으로 만들어줘 -1로 받은 이유는 그냥 구분을 위함...
            {
                for(int i = 0; i < 2; i++)
                { _resultCount[i]++; }
                pGuide.transform.Find("UI/Count/Count_P1").GetComponent<TMP_Text>().text = _resultCount[0].ToString();
                pGuide.transform.Find("UI/Count/Count_P2").GetComponent<TMP_Text>().text = _resultCount[1].ToString();

            }
            else if (_resultCount[0] >= 1 && _resultCount[1] >= 1) // 무제한이 아니라면..  --기 때문에 그대로 표시만 하면 됩니다.
            {
                //결과 값 위에 ChangeCount에서 받아옴 무한이 아니면 여기 >> _resultCount값 들은 위에 ChangeCount에서 메인에서 선택한 값을 받아와서 여기 적용
                pGuide.transform.Find("UI/Count/Count_P1").GetComponent<TMP_Text>().text = _resultCount[0].ToString();
                pGuide.transform.Find("UI/Count/Count_P2").GetComponent<TMP_Text>().text = _resultCount[1].ToString();
            }

            // 터치 리스트를 이중 리스트에 담아서 초기화 시켜줌
            for (int i = 0; i < TouchList.Length; i++)
            {
                TouchList[i] = new List<Transform>();
            }
            foreach (Transform t in pGuide.transform.Find("UI/Touch/P1/touch"))
            {
                TouchList[0].Add(t);
            }
            foreach (Transform t in pGuide.transform.Find("UI/Touch/P2/touch"))
            {
                TouchList[1].Add(t);
            }

            // 노말 스프라이트 마찬가지
            for (int i = 0; i < spriteList.Length; i++)
            {
                spriteList[i] = new List<Transform>();
            }
            foreach (Transform t in pGuide.transform.Find("UI/Button/n_P1"))
            {
                spriteList[0].Add(t);
            }
            foreach (Transform t in pGuide.transform.Find("UI/Button/n_P2"))
            {
                spriteList[1].Add(t);
            }

            // 파티클 리스트 마찬가지
            for (int i = 0; i < particleList.Length; i++)
            {
                particleList[i] = new List<Transform>();
            }
            foreach (Transform t in pGuide.transform.Find("UI/Particle/P1"))
            {
                particleList[0].Add(t);
            }
            foreach (Transform t in pGuide.transform.Find("UI/Particle/P2"))
            {
                particleList[1].Add(t);
            }

            // 처음 시작 조건 만들어두기 1. 양쪽 작은 버튼 터치 + disable 켜두기
            for(int i = 0; i <= 1; i ++)
            {
                spriteList[i][1].gameObject.GetComponent<SpriteRenderer>().sprite = nSprite[2];
                TouchList[i][1].gameObject.SetActive(false);
            }

            for (int i = 0; i < 2; i++) 
            { 
                spriteList[0][i].GetComponent<Animator>().enabled = false; spriteList[1][i].GetComponent<Animator>().enabled = false;

                guideUiTxtPlayer[i] = pGuide.transform.Find("UI/Text").GetChild(i).gameObject;
                guideUiCountTxt[i] = pGuide.transform.Find("UI/Count").GetChild(i).GetComponent<TMP_Text>();
                guidUiCountPlayer[i] = pGuide.transform.Find("UI/Count").GetChild(i).GetComponent<Animation>();

                for (int j = 0; j<2; j++)
                {
                    spritePlayerSpriteRender[i,j] = spriteList[i][j].GetComponent<SpriteRenderer>();
                    spriteListAni[i, j] = spriteList[i][j].GetComponent<Animator>();
                }
                    
            }

            resultAni = pResult.GetComponent<Animator>();
            resultCount1 = pResult.transform.Find("횟수1").GetChild(0).GetComponent<TMP_Text>();
            resultCount2 = pResult.transform.Find("횟수2").GetChild(0).GetComponent<TMP_Text>();
            guidTouch1Pause = pGuide.transform.Find("UI/Touch/Player_1_Pause_pan").gameObject;
            guidTouch2Pause = pGuide.transform.Find("UI/Touch/Player_2_Pause_pan").gameObject;

            soundEnd = _Sound.Find("Sound_End").GetComponent<AudioSource>();
            soundClick = _Sound.Find("Sound_Click").GetComponent<AudioSource>();
            soundSuccess = _Sound.Find("Sound_Success").GetComponent<AudioSource>();

            _isCoroutineRunningReleaseCheck = new bool[2, 10];
            _isCoroutineRunningStep = new bool[2];

            StartCoroutine(Stop());
        }

        public void On(int num)
        {
            int player = num > 0 ? 0 : 1;
            num = player == 0 ? num - 1 : Mathf.Abs(num + 1);

            _lasttime[player, num] = Time.time;

            if (TouchList[player][num].gameObject.activeSelf)
            {
                if (_press[player, num] == 0)
                {
                    soundClick.Play();
                }

                _press[player, num] = 1;
                // ReleaseCheck 코루틴의 중복 실행 방지
                if (!_isCoroutineRunningReleaseCheck[player, num])
                {
                    _isCoroutineRunningReleaseCheck[player, num] = true;
                    StartCoroutine(ReleaseCheck(player, num));
                }
                SpriteChange(player, num);
            }

            if (_step[player] == -1)
            {
                if (_press[player, 0] == 1)
                {
                    guideUiTxtPlayer[player].SetActive(false);
                    particleList[player][0].gameObject.SetActive(true);
                    TouchList[player][1].gameObject.SetActive(true);
                    spritePlayerSpriteRender[player, 1].sprite = nSprite[0];
                    TouchList[player][0].gameObject.SetActive(false);
                    spritePlayerSpriteRender[player, 0].sprite = nSprite[2];
                }

                _step[player] = _resultCount[player] == 0 ? -2 : 0;
            }

            // Step 코루틴의 중복 실행 방지
            if (_step[player] != -1 && !_isCoroutineRunningStep[player])
            {
                _isCoroutineRunningStep[player] = true;
                StartCoroutine(Step(player, num));
            }
        }

        private IEnumerator Step(int player, int num)  // 가기 전 준비 작업 + 반복 입성 제거
        {
            while (!_stepcheck[player])
            {
                _stepcheck[player] = true;
                yield return StartCoroutine(UpCheck(player, num));
            }
        }

        private IEnumerator UpCheck(int player, int num)     // 스텝을 체크하기 위한 코루틴  / 무한 이랑 구분
        {
            if ( _step[player] == 0)                            // 무제한이 아님
            {
                for (_returnStep[player] = 0; _returnStep[player] < 2; _returnStep[player]++)
                {                    
                    yield return StartCoroutine(UpStep(player,num,_returnStep[player]));
                }

                if (_resultCount[player] == 0)
                {
                    resultfirst(player);
                    for(int i = 0; i < 2; i++)
                    {
                        TouchList[player][i].gameObject.SetActive(false);
                        spritePlayerSpriteRender[player,i].sprite = nSprite[0];
                        _press[player, i] = 0;
                    }
                }

                
                if (guidTouch1Pause.activeSelf &&
                    guidTouch2Pause.activeSelf) resultview();

                _stepcheck[player] = false;
                yield break;
            }

            if (_step[player] <= -1)                        // 무제한임
            {
                for (_returnStep[player] = 0; _returnStep[player] < 2;  _returnStep[player]++)
                {
                    yield return StartCoroutine(UpStep(player,num,_returnStep[player]));
                }

                _stepcheck[player] = false;
                yield break;
            }
        }
       
        
        private IEnumerator UpStep(int player, int num, int sStep)       // 실질 적인 자세 부분을 위한 체크 switch사용
        {
            switch (sStep)
            {
                case 0:
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 3) i = 0;
                        yield return YieldInstructionCache.WaitForSeconds(0.1f);
                        if (_press[player, 1] == 1)
                        {
                            particleList[player][1].gameObject.SetActive(true);

                            TouchList[player][1].gameObject.SetActive(false);
                            spritePlayerSpriteRender[player,1].sprite = nSprite[2];

                            TouchList[player][0].gameObject.SetActive(true);
                            spritePlayerSpriteRender[player,0].sprite = nSprite[0];


                            particleList[player][0].gameObject.SetActive(false);
                            for (int n = 0; n < 2; n++) _press[player, n] = 0;

                            break;
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 3) i = 0;
                        yield return YieldInstructionCache.WaitForSeconds(0.1f);
                        if (_press[player, 0] == 1)
                        {
                            particleList[player][0].gameObject.SetActive(true);

                            TouchList[player][1].gameObject.SetActive(true);
                            spritePlayerSpriteRender[player,1].sprite = nSprite[0];

                            TouchList[player][0].gameObject.SetActive(true);
                            spritePlayerSpriteRender[player,0].sprite = nSprite[2];

                            particleList[player][1].gameObject.SetActive(false);

                            for (int n = 0; n < 2; n++) _press[player, n] = 0;

                            if (_step[player] == 0) _resultCount[player]--;
                            else _resultCount[player]++;

                            soundSuccess.Play();
                            guideUiCountTxt[player].text = _resultCount[player].ToString();
                            guidUiCountPlayer[player].Play();
                            break;
                        }
                    }
                    break;
            }
            yield return null;
        }
        
        
        IEnumerator ReleaseCheck(int player, int num)  // 전역 변수랑 지역 변수 시간이 같아지면 press가 그만되었단 소리
        {
            float rtime = _lasttime[player, num];
            yield return YieldInstructionCache.WaitForSeconds(0.1f); // 0.1초로 수정해야함.
            if (rtime == _lasttime[player, num ])
            {
               _press[player, num] = 0;

                spriteListAni[player,num].StopPlayback();
                spriteListAni[player,num].enabled = false;
                if(spritePlayerSpriteRender[player,num].sprite != nSprite[2] ) SpriteChange(player, num);
            }
        }
        
        private void SpriteChange(int player, int num) //_press안의 인덱스가 1이 되면 프레스 스프라이트가 켜지고 노말이 꺼짐 0 이면 반대
        {
            if(spritePlayerSpriteRender[player, num].sprite == nSprite[2]
                || spriteListAni[player,num].enabled == true) return;

            if (_press[player, num] == 1)
            {
                if (spritePlayerSpriteRender[player, num].sprite == nSprite[0])
                {
                    spritePlayerSpriteRender[player, num].sprite = nSprite[1];
                    spriteListAni[player,num].enabled = true;
                    spriteListAni[player,num].Play("Holding");
                }
            }
            else if (_press[player, num] == 0)
            {
                if (spritePlayerSpriteRender[player, num].sprite == nSprite[1])
                {
                    spritePlayerSpriteRender[player, num].sprite = nSprite[0];
                }
            }
        }

        /*
         만약에 manager의 enum값에 그 사람의 결과 값을 넣어줘야 한다면 여기다가 인당 _Setplay를 넣어주거나
        운동 종료 시 (ex 둘다 끝냈을 때) 넣어주면 된다.

        + 무제한도 포함이라면 _Setplay가 아니라 _resultcount값을 절대값으로 바꿔서 넣어주면 된다.
         */

        private void resultfirst(int player)
        {
            if(player == 0)
            {
                guidTouch1Pause.SetActive(true);
            }

            if(player == 1)
            {
                guidTouch2Pause.SetActive(true);
            }
        }

        private void resultview()
        {
            StopAllCoroutines();
            Destroy(pGuide);
            soundEnd.Play();
            pResult.SetActive(true);

            resultCount1.text = _SetPlay.ToString();
            resultCount2.text = _SetPlay.ToString();

            resultAni.Play("2인_횟수");
        }
    }
}
