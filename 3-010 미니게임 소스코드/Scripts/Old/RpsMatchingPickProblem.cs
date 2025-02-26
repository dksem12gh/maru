using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingPickProblem : MonoBehaviour
    {
        [SerializeField] GamePlayUI playUi;
        [SerializeField] RpsMatchingPlayerManager playerMgr;

        public Queue<int> rpsQueue;
        public Queue<int> ruleQueue;
        
        public bool Completed
        {
            get 
            {
                int count = ruleQueue.Count;

                return count <= 0 ? true : false;
            }
        }

        public int Current
        {
            get { return ruleQueue.Peek(); }
        }

        public void Next()
        {
            ruleQueue.Dequeue();
            rpsQueue.Dequeue();
        }

        public void StartQuestion()
        {            
            StartCoroutine(BeginQuestionCreate());
        }

        public bool SubmitAnswer(int index)
        {
            int result = DetermineResult(index,rpsQueue.Peek());
            bool isCorrect = (ruleQueue.Peek() == 0 && result == 0) ||
                             (ruleQueue.Peek() == 1 && result == 1) ||
                             (ruleQueue.Peek() == 2 && result == 2);

            return isCorrect;
        }

        public IEnumerator DelayTimeCorutine(float time)
        {
            yield return YieldInstructionCache.WaitForSeconds(time);
        }

        public List<int> CreateQuizList(RpsMatchingSettings quizMax)
        {
            List<int> temp = GenerateQuizsByRandoms(quizMax.GameCountGoal);
            ruleQueue = new Queue<int>(temp);
            temp = GenerateQuizsByRandoms(quizMax.GameCountGoal);
            rpsQueue = new Queue<int>(temp);
            return temp;
        }

        private List<int> GenerateQuizsByRandoms(int quizMax)
        {
            List<int> temp = new List<int>();

            for (int i = 0; i < quizMax; i++)
            {
                temp.Add(Random.Range(0,3));
            }
            return temp;
        }

        private IEnumerator BeginQuestionCreate()
        {
            yield return DelayTimeCorutine(0.5f);

            if (ruleQueue.Count > 0)
            {
                //설정된 문제 순차적으로 보여주기 애니메이션
                playerMgr.StageEnd();
                yield return StartCoroutine(playUi.PlayGame(rpsQueue.Peek(), ruleQueue.Peek()));
                playerMgr.StageStart();
                //_PausePan.gameObject.SetActive(false);
                //sound.Play(Guessingnumberorder.SoundIndex.Question);
            }
        }

        public int DetermineResult(int playerSelect,int QuestionRule)
        {
            if (playerSelect == QuestionRule)
                return 2; // 무승부

            if ((playerSelect == 0 && QuestionRule == 2) ||
                (playerSelect == 1 && QuestionRule == 0) ||
                (playerSelect == 2 && QuestionRule == 1))
                return 0; // 플레이어 승리

            return 1; // 컴퓨터 승리
        }

        /*public IEnumerator d*/
    }
}