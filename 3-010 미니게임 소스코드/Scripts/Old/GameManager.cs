using System.Collections;
using UnityEngine;
using TouchScript.Examples.RawInput;

/*
 * 1. 보기 칸은 4개
 * 2. 난이도에 따라 랜덤으로 깜빡이는 순서가 늘어난다.
 * 3. 플레이어는 랜덤으로 깜빡인 순서대로 발판을 눌러야한다.
 * 4. 입력도중 틀리면 바로 틀려야한다.
 * 
 */
namespace DigitalMaru.MiniGame.RockPaperScissors
{
    internal class GameInfo
    {
        public const int Player1 = 0;
        public const int Player2 = 1;
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject pTitle;
        public GameObject _PausePan;
        public Camera _GameCamera;

        private int AnswerKey = 0;

        //        [SerializeField] Guessingnumberorder.SoundPlayer SoundPlay;
        //        [SerializeField] Guessingnumberorder.GameResultUI ResultUI;
        [SerializeField] GamePlayUI PlayUI;
        [SerializeField] GamePlayer[] Players;

        int CurrentQuestion;

        void Awake()
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            MultiDisplayTouchManager multiDisplayTouchManager = FindObjectOfType<MultiDisplayTouchManager>();
            multiDisplayTouchManager.MainCam[1] = _GameCamera;
        }


        private void OnDisable()
        {
            if (StopAsnc != null)
            {
                StopCoroutine(StopAsnc);
            }
        }
        private void OnDestroy()
        {
            Players = null;
        }

        //일시정지 : 왜 이렇게 하는지 모르겠지만,
        //           다른 게임도 이럴꺼기 때문에, 그때 일괄적용합시다.
        IEnumerator StopAsnc = null;
        private IEnumerator Stop()
        {
            while (true)
            {
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
                while (Managers.SelGameSet.selectGameState == SelectGameState.Pause)
                {
                    _PausePan.gameObject.SetActive(true);
                    yield return YieldInstructionCache.WaitForSeconds(0.1f);
                }
                _PausePan.gameObject.SetActive(false);
            }
        }


        private void Start()
        {
            //타이틀 보여주며 시작
            StartCoroutine(EntryGame());
        }

        private IEnumerator EntryGame()
        {
            // Show Title
            GameObject TitleObj = Instantiate(pTitle, _GameCamera.transform);
            yield return YieldInstructionCache.WaitForSeconds(4.3f);
            Destroy(TitleObj);
            TitleObj = null;


            _PausePan.gameObject.SetActive(true);

            // Init Game
            CurrentQuestion = Managers.GameTime;


            if (StopAsnc != null)
            {
                StopCoroutine(StopAsnc);
            }
            StopAsnc = Stop();
            StartCoroutine(StopAsnc);

            yield return StartCoroutine(PrepareGame(0.1f));
            StartCoroutine(BeginQuestion());
        }

        private IEnumerator PrepareGame(float delayTime = 1.0f)
        {
            RecvResultPlayer = 0;

            foreach (var player in Players)
                player.PrepareGame();

            yield return YieldInstructionCache.WaitForSeconds(delayTime);

            PlayUI.PrepareGame();
        }

        int QuestShapeId;
        int QuestRuleId;
        int[,] QuestMapping = { { 1, 2, 0 }, { 2, 0, 1 }, { 0, 1, 2 } };

        private IEnumerator BeginQuestion()
        {
            // 문제 답안 생성
            {
                QuestShapeId = Random.Range(0, 3);
                QuestRuleId = Random.Range(0, 3);

                AnswerKey = QuestMapping[QuestRuleId, QuestShapeId];
            }

            yield return YieldInstructionCache.WaitForSeconds(0.5f);//애니메이션 끝날때 까지 기다려주는거            

            if (CurrentQuestion-- > 0)
            {
                //설정된 문제 순차적으로 보여주기 애니메이션
                yield return StartCoroutine(PlayUI.PlayGame(QuestShapeId, QuestRuleId));

                _PausePan.gameObject.SetActive(false);


                //                SoundPlay.Play(Guessingnumberorder.OldSoundIndex.Question);

                foreach (var player in Players)
                {
                    player.PlayGame(AnswerKey);
                }
            }
            else
            {
                //                SoundPlay.Play(Guessingnumberorder.OldSoundIndex.GameEnd);

                PlayUI.DisableCanvas();
                //ResultUI.Show(Players[GameInfo.Player1].CurrentScore,
                //               Players[GameInfo.Player2].CurrentScore);
            }

        }

        int RecvResultPlayer = 0;
        public void UpdatePlayerResult(int playerId, int oxId)
        {
            RecvResultPlayer++;

            switch (oxId)
            {
                case OrderState.InCorrect:
                    //                    SoundPlay.Play(Guessingnumberorder.OldSoundIndex.Failure);

                    PlayUI.ShowPlayerResult(playerId, OrderState.InCorrect);

                    // 둘다 실패했을 경우
                    if (RecvResultPlayer > 1)
                    {
                        StartCoroutine(NextQuestion());
                    }
                    break;

                case OrderState.Correct:
                    //                    SoundPlay.Play(Guessingnumberorder.OldSoundIndex.Success);

                    Players[playerId].AddScore();
                    PlayUI.ShowPlayerResult(playerId, OrderState.Correct);
                    PlayUI.UdpatePlayerScore(playerId, Players[playerId].CurrentScore);

                    // 한명이 먼저 성공한 경우
                    StartCoroutine(NextQuestion());
                    break;
            }
        }

        private IEnumerator NextQuestion()
        {
            yield return StartCoroutine(PrepareGame(1.5f));
            StartCoroutine(BeginQuestion());
        }
    }
}
