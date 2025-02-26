using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] Canvas canvas;

        //        [SerializeField] Guessingnumberorder.SoundPlayer soundPlay;

        [SerializeField] Animator RPSShapeAnim;
        [SerializeField] Image[] RPSShapeImg;
        [SerializeField] Animator RPSRuleAnim;
        [SerializeField] Image[] RPSRuleImg;

        [SerializeField] TMP_Text[] ScoreText;
        [SerializeField] GameObject[] xObj;
        [SerializeField] GameObject[] oObj;

        void Start()
        {
            DisableCanvas();

            foreach (var x in RPSShapeImg)
                x.enabled = false;

            foreach (var x in RPSRuleImg)
                x.enabled = false;

            for (int p = 0; p < 2; p++)
            {
                ScoreText[p].text = string.Format("{0:00}", 0);
            }
        }


        private void OnDestroy()
        {

        }

        public void PrepareGame()
        {
            canvas.enabled = true;

            //모든 버튼 false
            for (int p = 0; p < 2; p++)
            {
                oObj[p].SetActive(false);
                xObj[p].SetActive(false);
            }
        }

        [SerializeField] float ShuffleTotalTime = 1.5f;
        [SerializeField] float ShuffleNextTime = 0.05f;
        public IEnumerator PlayGame(int QuestShapeId, int QuestRuleId)
        {
            // 가바보 규칙을 먼저하고
            float elapasedTime = 0f;

            RPSRuleAnim.Play("IDLE");
            while (true)
            {
                foreach (Image x in RPSRuleImg)
                    x.enabled = false;


                if (elapasedTime < ShuffleTotalTime)
                {
                    // 랜덤으로 텍스트 선택                
                    RPSRuleImg[UnityEngine.Random.Range(0, RPSRuleImg.Length)].enabled = true;
                }
                else
                {
                    RPSRuleImg[QuestRuleId].enabled = true;
                    break;
                }

                elapasedTime += Time.smoothDeltaTime;
                yield return YieldInstructionCache.WaitForSeconds(ShuffleNextTime);
            }


            RPSRuleAnim.Play("SELECT");
            elapasedTime = 0;
            yield return YieldInstructionCache.WaitForSeconds(0.5f);

            RPSRuleAnim.Play("START");

            // 가바보 모양을 결정한다.
            RPSShapeAnim.Play("IDLE");
            while (true)
            {
                foreach (Image x in RPSShapeImg)
                    x.enabled = false;


                if (elapasedTime < ShuffleTotalTime)
                {
                    // 랜덤으로 텍스트 선택                
                    RPSShapeImg[UnityEngine.Random.Range(0, RPSShapeImg.Length)].enabled = true;
                }
                else
                {
                    RPSShapeImg[QuestShapeId].enabled = true;
                    break;
                }

                elapasedTime += Time.smoothDeltaTime;
                yield return YieldInstructionCache.WaitForSeconds(ShuffleNextTime);
            }

            RPSShapeAnim.Play("SELECT");

            yield return YieldInstructionCache.WaitForSeconds(0.5f);

            RPSShapeAnim.Play("RPS_SELECT");
        }


        public void ShowPlayerResult(int playerId, int oxId)
        {
            switch (oxId)
            {
                case 0: xObj[playerId].gameObject.SetActive(true); break;
                case 1: oObj[playerId].gameObject.SetActive(true); break;
            }
        }

        public void UdpatePlayerScore(int playerId, int score)
        {
            ScoreText[playerId].text = string.Format("{0:00}", score);
        }

        public void DisableCanvas()
        {
            canvas.enabled = false;
        }


    }
}