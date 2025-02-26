using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsQuestionDisplay : MonoBehaviour
    {
        [Header("Animators")]
        [SerializeField] Animator RPSShapeAnim;
        [SerializeField] Animator RPSRuleAnim;

        [Header("Images")]
        [SerializeField] List<Image> RPSShapeImages;
        [SerializeField] TMP_Text RPSRuleTxt;

        WaitForSecondsWithPause wait;

        string[] rule = new string[3];
        string[] addRule = new string[3];

        public async void PrepareUI()
        {
            foreach (var shapeImage in RPSShapeImages)
                shapeImage.enabled = false;

            string[] ruleKeys = { "Win", "Draw", "Lose" };
            string[] addRuleKeys = { "DWin", "DDraw", "DLose" };

            for (int i = 0; i < ruleKeys.Length; i++)
            {
                rule[i] = await GetLocalizedString(ruleKeys[i]);
            }
            

            for (int i = 0; i < addRuleKeys.Length; i++)
            {
                addRule[i] = await GetLocalizedString(addRuleKeys[i]);
            }

            /*foreach (var ruleImage in RPSRuleImages)
                ruleImage.enabled = false;*/

            wait = new WaitForSecondsWithPause(this);
        }

        private async Task<string> GetLocalizedString(string key)
        {
            return await LocalizationSettings.StringDatabase.GetLocalizedStringAsync("MinigameText", key);
        }

        float ShuffleTotalTime = 1.5f;
        float ShuffleNextTime = 0.02f;

        public IEnumerator ShowQuestion(Question question, int index,int level)
        {
            MiniGameRunnerPauseProvider.Instance.PauseChangedEvent += OnPause;
            List<ShapeType> shapes = question.GetShapeTypes();
            List<RuleType> rules = question.GetRules();            

            float elapasedTime = 0f;

            RPSRuleAnim.Play("IDLE");
            RPSShapeAnim.Play("IDLE");
            while (true)
            {
                /*foreach (var image in RPSRuleImages)
                    image.enabled = false;*/

                int rand = Random.Range(0, 2);
                int ruleRand = Random.Range(0, (int)RuleType.None);
                int ruleAddRand;                

                // ruleRand와 중복되지 않도록 ruleAddRand를 설정
                do
                {
                    ruleAddRand = Random.Range(0, (int)RuleType.None);
                } while (ruleAddRand == (int)rules[index]);



                foreach (var image in RPSShapeImages)
                    image.enabled = false;


                if (elapasedTime < ShuffleTotalTime)
                {
                    RPSShapeImages[Random.Range(0, (int)ShapeType.None)].enabled = true;

                    if (level <= 1)
                    {
                        RPSRuleTxt.text = rule[ruleRand];
                    }
                    else
                    {
                        RPSRuleTxt.text = addRule[ruleAddRand] +" "+ rule[ruleRand];
                    }
                    //RPSRuleImages[Random.Range(0, (int)RuleType.None)].enabled = true;
                }
                else
                {
                    if (level <= 1)
                    {
                        RPSRuleTxt.text = rule[(int)rules[index]];
                    }
                    else
                    {
                        if (rand == 0)
                        {
                            RPSRuleTxt.text = addRule[ruleAddRand] + " " + rule[(int)rules[index]];
                        }
                        else
                        {
                            RPSRuleTxt.text = rule[(int)rules[index]];
                        }
                    }
                    //RPSRuleImages[(int)rules[index]].enabled = true;
                    RPSShapeImages[(int)shapes[index]].enabled = true;

                    break;
                }

                elapasedTime += Time.smoothDeltaTime;
                yield return wait.Run(ShuffleNextTime);
            }
            RPSRuleAnim.Play("SELECT");
            RPSShapeAnim.Play("SELECT");

            yield return wait.Run(0.5f);

            RPSRuleAnim.Play("START");
            RPSShapeAnim.Play("RPS_SELECT");

            MiniGameRunnerPauseProvider.Instance.PauseChangedEvent -= OnPause;
        }

        public void DisableUI()
        {
            foreach (var image in RPSShapeImages)
                image.enabled = false;

            /*foreach (var image in RPSRuleImages)
                image.enabled = false;*/
            RPSRuleTxt.text = "";
        }

        private void OnPause(bool pause)
        {
            wait.Pause(pause);

            if (pause)
            {
                RPSShapeAnim.speed = 0f;
                RPSRuleAnim.speed = 0f;
            }
            else
            {
                RPSShapeAnim.speed = 1f;
                RPSRuleAnim.speed = 1f;
            }
        }
    }
}