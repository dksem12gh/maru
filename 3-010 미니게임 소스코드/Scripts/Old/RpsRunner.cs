using UnityEngine;
using System.Collections;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsRunner : MonoBehaviour
    {
        [SerializeField] int id;

        [SerializeField] GameManager gameManager;
        //        [SerializeField] Guessingnumberorder.SoundPlayer soundPlay;

        [SerializeField] TouchEventHandler[] TouchEvent;
        [SerializeField] Animator[] ButtonAnim;

        const int MAX_TOUCH = 3;

        public int CurrentScore { get; private set; }

        void Start()
        {
            CurrentScore = 0;
            InitPlayerButton();
        }


        private void OnDestroy()
        {
            TouchEvent[0].m_PressedEvent.RemoveListener(val => ProcessButton(0));
            TouchEvent[1].m_PressedEvent.RemoveListener(val => ProcessButton(1));
            TouchEvent[2].m_PressedEvent.RemoveListener(val => ProcessButton(2));
        }


        public void PrepareGame()
        {
            DisablePlayerButton();
        }


        int AnswerKey;
        public void PlayGame(int Key)
        {
            AnswerKey = Key;
            HoldButton = false;

            EnablePlayerButton();
        }

        public void EnablePlayerButton()
        {
            for (int i = 0; i < MAX_TOUCH; i++) //모든 버튼 누를수 있게
                ButtonAnim[i].Play("Alacrity Test_idle");
        }

        public void DisablePlayerButton()
        {
            for (int i = 0; i < MAX_TOUCH; i++) //박스콜리더 모두 끄기
                ButtonAnim[i].Play("Alacrity Test_idle(disable)");
        }

        public void InitPlayerButton()
        {
            TouchEvent[0].m_PressedEvent.AddListener(val => ProcessButton(0));
            TouchEvent[1].m_PressedEvent.AddListener(val => ProcessButton(1));
            TouchEvent[2].m_PressedEvent.AddListener(val => ProcessButton(2));


            for (int i = 0; i < MAX_TOUCH; i++)
                ButtonAnim[i].Play("Alacrity Test_idle(disable)");
        }


        void ProcessButton(int index)
        {
            if (HoldButton) return;


            //버튼 커지는 애니메이션 실행            
            //            soundPlay.Play(Guessingnumberorder.SoundIndex.Click);            
            ButtonAnim[index].Play("Alacrity Test_Clickbutton");

            StartCoroutine(HoldButtonProcess());

            // 현재 입력한 버튼 순서와 답안이 다를 경우
            if (AnswerKey != index)
            {
                // 현재 순서 카드 틀림 표시
                StartCoroutine(UpdatePlayerState(OrderState.InCorrect));
            }
            else
            {
                // 현재 순서 카드 맞음 표시                    
                StartCoroutine(UpdatePlayerState(OrderState.Correct));
            }
        }

        // 버튼을 눌렀을때 버튼 애니메이션 길이가 1.0 초
        bool HoldButton = false;
        private IEnumerator HoldButtonProcess()
        {
            HoldButton = true;

            yield return YieldInstructionCache.WaitForSeconds(1.0f);

            HoldButton = false;
        }

        private IEnumerator UpdatePlayerState(int OrderState)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.5f);

            DisablePlayerButton();

            // O,  X 출력
            gameManager.UpdatePlayerResult(id, OrderState);
        }

        public void AddScore()
        {
            CurrentScore++;
        }
    }
}