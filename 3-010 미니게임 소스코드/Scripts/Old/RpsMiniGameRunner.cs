using System.Collections;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMiniGameRunner : MiniGameRunner
    {
        [SerializeField] private RpsRule rps;
        bool IsPlaying { get; set; } = false;
        
        public override void Prepare()
        {
            rps.Prepare();
        }

        public override IEnumerator Run()
        {
            Play();
            yield return WaitUntilCompleted();
            Stop();

            void Play()
            {
                IsPlaying = true;
                rps.Do();
            }

            void Stop()
            {
                IsPlaying = false;                
            }

            IEnumerator WaitUntilCompleted()
            {
                yield return YieldInstructionCache.WaitUntil(() =>
                {
                    return rps.Completed;
                });
            }
        }


        protected override void PauseHandle(bool pause)
        {
            if (IsPlaying is false) return;
            rps.Pause(pause);
        }

        public override MiniGameResultState GetResult()
        {
            var scoreArray = rps.GetResultData();
            if (scoreArray == null) return MiniGameResultState.Draw;
            if (scoreArray.Length < 2) return MiniGameResultState.Draw;

            if (scoreArray[0] > scoreArray[1]) return MiniGameResultState.Win1P;
            if (scoreArray[0] < scoreArray[1]) return MiniGameResultState.Win2P;
            return MiniGameResultState.Draw;
        }
    }
}
















