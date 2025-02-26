using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsPlayer : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] RpsPlayerCount playerCount;
        [SerializeField] List<RpsPlayerButton> buttons;

        Answer answer;

        public float swapInterval = 0.1f; // 스왑 간격
        public float moveDuration = 0.1f; // 이동 시간

        public bool IsSuccessed
        {
            get;
            private set;
        }

        public bool IsFailed
        {
            get;
            private set;
        }

        int score = 0;

        public int GetScore() => score;

        public void Prepare()
        {
            score = 0;

            IsSuccessed = false;
            IsFailed = false;

            playerCount.ScoreUpdate(score);

            foreach (var button in buttons)
            {
                button.SpriteChange(SpriteIndex.Disabled);
            }
        }

        public void StartAnswer(Answer answer,int level)
        {
            this.answer = answer;

            foreach (var button in buttons)
            {
                button.SpriteChange(SpriteIndex.Normal);
                button.ActivateInput(true);
            }
        }
        public void EndAnswer()
        {
            foreach(var button in buttons)
            {
                button.ActivateInput(false);
            }
        }

        public void ResetAnswer()
        {
            foreach (var button in buttons)
            {
                button.SpriteChange(SpriteIndex.Disabled);
            }

            IsSuccessed = false;
            IsFailed = false;
        }

        public void Successed(RpsPlayerButton button)
        {
            IsSuccessed = true;

            score += 1;

            playerCount.ScoreUpdate(score);

            button.Success();

            RpsSoundManager.instance.Play(SoundIndex.Success);
        }

        public void Failed(RpsPlayerButton button)
        {
            IsFailed = true;

            button.Failed();

            RpsSoundManager.instance.Play(SoundIndex.Failure);
        }

        public void OnSelectedButton(RpsPlayerButton button)
        {
            foreach (var each in buttons)
            {
                each.ActivateInput(false);
                each.SpriteChange(SpriteIndex.Disabled);
            }

            button.Pressed();

            if(answer.CheckAnswer(button.GetShape()))
                Successed(button);
            else
                Failed(button);
        }

        private void OnPause(bool pause)
        {
            if (IsFailed || IsSuccessed)
                return;

            foreach(var each in buttons)
            {
                each.ActivateInput(pause);
            }
        }

        public void SwapRpsButtons()
        {
            StartCoroutine(SwapObjects());
        }

        IEnumerator SwapObjects()
        {
            // 오브젝트 스왑 애니메이션 실행
            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);

            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);

            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);
            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);

            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);

            yield return StartCoroutine(Swap());
            // 지정된 간격만큼 대기
            yield return new WaitForSeconds(swapInterval);
        }

        IEnumerator Swap()
        {
            int indexA = UnityEngine.Random.Range(0, buttons.Count);
            int indexB;
            do
            {
                indexB = UnityEngine.Random.Range(0, buttons.Count);
            } while (indexA == indexB);

            Vector3 startPosA = buttons[indexA].transform.localPosition;
            Vector3 startPosB = buttons[indexB].transform.localPosition;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                // 시간에 따라 위치 보간
                float t = elapsedTime / moveDuration;
                buttons[indexA].transform.localPosition = Vector3.Lerp(startPosA, startPosB, t);
                buttons[indexB].transform.localPosition = Vector3.Lerp(startPosB, startPosA, t);
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // 최종 위치 설정
            buttons[indexA].transform.localPosition = startPosB;
            buttons[indexB].transform.localPosition = startPosA;
        }
    }
}