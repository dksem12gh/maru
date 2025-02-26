using UnityEngine;
using UnityEngine.Events;


namespace DigitalMaru.MiniGame.RockPaperScissors
{

    public class RpsMatchingPlayer : MonoBehaviour
    {
        public enum STATE_KIND
        {
            NONE,
            SUCCESS,
            FAILED,
        }

        [Header("컴포넌트")]
        [SerializeField] RpsMatchingResultAnim miniGameResultAnim;
        [SerializeField] RpsMatchingPadContainer touchPad;
        [SerializeField] RpsMiniGameScore score;

        [Header("변수")]
        [SerializeField] PlayerNumber playerNumber;

        public STATE_KIND State { get; private set; } = STATE_KIND.NONE;

        public UnityEvent TouchEvent = new UnityEvent();
        public UnityEvent SuccessEvent = new UnityEvent();
        public UnityEvent FailedEvent = new UnityEvent();
        public UnityEvent PrepareEvent = new UnityEvent();

        public PlayerNumber Number => playerNumber;
        public bool IsReady { get; private set; } = false;
        public RpsMatchingPadContainer Container => touchPad;

        public int playerAnswer;        
        private IRpsSelecter rpsMatchingSelecter;

   
        private void OnEnable()
        {
            touchPad.TouchEvent += Touch;
        }

        private void OnDisable()
        {
            touchPad.TouchEvent -= Touch;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active); 
        }

        public void SetSelecter(IRpsSelecter selecter)
        {
            this.State = STATE_KIND.NONE;
            this.rpsMatchingSelecter = selecter;          
        }
                
        public void Pause(bool pause)
        {
            if (pause)
                touchPad.DisableTouch();
            else
                touchPad.EnableTouch();
        }

        public void Success()
        {
            SuccessEvent?.Invoke();
            miniGameResultAnim.PlaySuccess();
            score.ScoreUp();
            touchPad.DisableTouch();
            State = STATE_KIND.SUCCESS;
        }

        public void Fail()
        {            
            FailedEvent?.Invoke();
            miniGameResultAnim.PlayFail();
            touchPad.DisableTouch();
            State = STATE_KIND.FAILED;
        }

        public void StageReady()
        {
            this.State = STATE_KIND.NONE;
            miniGameResultAnim.Stop();
            touchPad.DisableTouch();
        }

        public void StageStart()
        {            
            touchPad.EnableTouch();
        }

        public void StageEnd()
        {
            touchPad.DisableTouch();
        }

        public int ToResultData()
        {
            return score.ToResultData();
        }

        public void ToAnswerPlayer(int answer)
        {
            playerAnswer = answer;
        }

        private void Touch(int value)
        {
            TouchEvent?.Invoke();
            rpsMatchingSelecter?.SubmitAnswer(Number, value);
        }
    }
}