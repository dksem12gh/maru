using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalMaru.Network;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class WaitForSecondsWithPause
    {
        float delay = 0f;
        float time = 0f;
        bool running = false;
        readonly MonoBehaviour mono;

        Coroutine runRutine;

        public WaitForSecondsWithPause(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public IEnumerator Run(float delay)
        {
            time = 0f;
            running = true;
            this.delay = delay;
            runRutine = mono.StartCoroutine(RunRoutine());
            yield return YieldInstructionCache.WaitUntil(() => !running);
        }

        public IEnumerator RunRoutine()
        {
            while (time <= delay)
            {
                time += Time.deltaTime;
                yield return null;
            }
            running = false;
            runRutine = null;
        }

        public void Pause(bool pause)
        {
            if (running is false) return;

            if (pause)
            {
                if (runRutine != null)
                {
                    mono.StopCoroutine(runRutine);
                    runRutine = null;
                }
            }
            else
            {
                runRutine = mono.StartCoroutine(RunRoutine());
            }
        }
    }

    public class RpsGamePlayManager : MonoBehaviour
    {
        [SerializeField] ProcessCommandLine _processCommandLine;

        [Header("Components")]
        [SerializeField] List<RpsPlayer> players;
        [SerializeField] RpsQuestionManager questionManager;

        [Header("Canvas")]
        [SerializeField] Canvas canvas;
        [SerializeField] BoxCollider blockObj;
        
        WaitForSecondsWithPause wait;

        int count;

        int playerCount = 0;

        int level = 0;

        private void OnDestroy()
        {
            MiniGameRunnerPauseProvider.Instance.Dispose();
        }

        public void Prepare()
        {
            wait = new WaitForSecondsWithPause(this);

            switch(ProcessCommandLine.ContentLevel)
            {
                case 1:                    
                    level = 1;
                    break;
                case 2:                    
                    level = 2;
                    break;
                case 3:                    
                    level = 3;
                    break;
            }

            count = 10;

            questionManager.PrepareUI();
            
            questionManager.CreateRandomList(count);

            if ((int)ProcessCommandLine.ContentPlayerMax == 1)
            {
                playerCount = 0;
            }
            else
            {
                playerCount = 1;
            }

            if (playerCount == 0)
            {
                players[1].transform.gameObject.SetActive(false);
                players.RemoveAt(1);
                players[0].transform.localPosition = new Vector3(0, -5.2f, 0);                
            }

            foreach(var player in players)
            {
                player.Prepare();
            }

            canvas.enabled = false;
        }

        public IEnumerator Run()
        {
            canvas.enabled = true;

            for (int i = 0; i < count; i++)
            {
                blockObj.enabled = true;

                var question = questionManager.GenerateQuestion(i);

                if (level == 3)
                {
                    foreach (var player in players)
                    {
                        player.SwapRpsButtons();
                    }
                }

                yield return questionManager.ShowQuestionAnimation(i,level);

                foreach(var player in players)
                {
                    player.StartAnswer(question,level);
                }

                blockObj.enabled = false;

                yield return new WaitUntil(() => IsBothPlayerEnd() || IsPlayerSuccessed());

                foreach(var player in players)
                {
                    player.EndAnswer();
                }

                yield return wait.Run(1f);

                foreach(var player in players)
                {
                    player.ResetAnswer();
                }

                questionManager.EndQuestion();
            }

            bool IsBothPlayerEnd()
            {
                foreach(var player in players)
                {
                    if (!player.IsFailed || player.IsSuccessed)
                        return false;
                }

                return true;
            }

            bool IsPlayerSuccessed()
            {
                foreach (var player in players)
                {
                    if (player.IsSuccessed)
                        return true;
                }

                return false;
            }
        }

        public void OnPause(bool pause)
        {
            MiniGameRunnerPauseProvider.Instance.Pause(pause);
            wait.Pause(pause);
        }

        public MiniGameResultState GetResult() => CheckResult();

        private MiniGameResultState CheckResult()
        {
            if (playerCount == 1)
            {
                if (players[0].GetScore() > players[1].GetScore())
                    return MiniGameResultState.Win1P;
                else if (players[0].GetScore() < players[1].GetScore())
                    return MiniGameResultState.Win2P;
                else
                    return MiniGameResultState.Draw;
            }
            else
            {
                return MiniGameResultState.PlayerOne;
            }
        }
    }
}