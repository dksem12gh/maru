using System.Collections;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public interface IRpsSelecter
    {
        void SubmitAnswer(PlayerNumber playerNum, int value);
    }

    public class RpsRule : MiniGameRule, IRpsSelecter
    {
        [Header("플레이어들")]
        [SerializeField] RpsMatchingPlayerManager playerMgr;
        [Header("문제")]
        [SerializeField] RpsMatchingPickProblem pickProblem;
        [Header("변수")]
        [SerializeField] float readyWaitSec;
        [SerializeField] float startWaitTime;
        [SerializeField] float resultWaitTime;

        [SerializeField] private GamePlayUI playUi;
        //        [SerializeField] private Guessingnumberorder.SoundPlayer sound;

        public override bool Completed
        {
            get
            {
                if (pickProblem.Completed)
                {
                    CompletedEvent?.Invoke();
                    return true;
                }
                return false;
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void OnEnable()
        {
            playerMgr.SetSelecter(this);
        }

        public void SubmitAnswer(PlayerNumber playerNum, int value)
        {
            if (pickProblem.SubmitAnswer(value))
            {
                Success(playerNum);
            }
            else
            {
                Fail(playerNum);
            }

            void Success(PlayerNumber number)
            {
                SuccessEvent?.Invoke();
                playerMgr.GetPlayer(number).Success();
            }

            void Fail(PlayerNumber number)
            {
                FailEvent?.Invoke();
                playerMgr.GetPlayer(number).Fail();
            }
        }

        public override void Do()
        {
            StopAllCoroutines();
            StartCoroutine(DoRun());
        }

        public override void Begin()
        {
            //StartCoroutine(BeginQuestionCreate());            
        }
        public override void Prepare()
        {
            pickProblem.CreateQuizList(new RpsMatchingSettings(playerMgr.ChoiceRange));
            playerMgr.Prepare();
        }

        public override void Pause(bool _pause)
        {
            playUi.DisableCanvas();
        }

        public override int[] GetResultData()
        {
            return playerMgr.GetResultData();
        }

        private IEnumerator DoRun()
        {
            StartEvent?.Invoke();

            RunEvent?.Invoke();
            while (this.Completed is false)
            {
                yield return DoStageReady();
                yield return DoStageStart();
                yield return DoStageEnd();
                pickProblem.Next();
            }
        }

        private IEnumerator DoStageReady()
        {
            playerMgr.StageReady();
            yield return YieldInstructionCache.WaitForSeconds(readyWaitSec);
        }

        private IEnumerator DoStageStart()
        {
            yield return YieldInstructionCache.WaitForSeconds(startWaitTime);
            playUi.PrepareGame();
            pickProblem.StartQuestion();
            //playerMgr.StageStart();
        }

        private IEnumerator DoStageEnd()
        {
            playerMgr.StageEnd();
            yield return new WaitUntil(() => playerMgr.IsResult());
            yield return YieldInstructionCache.WaitForSeconds(resultWaitTime);
        }
    }
}
