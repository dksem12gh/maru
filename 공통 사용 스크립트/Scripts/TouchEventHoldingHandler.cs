using System.Collections;
using UnityEngine;

namespace DigitalMaru
{
    /// <summary>
    /// 라이더에서는 Hold 기능 처리가 안됨.
    /// 라이더는 짧은 시간안에 Enter와 Exit가 일어나기 때문에, Hold를 따로 처리해 줘야 함.
    /// 딜레이를 줘서, 이 이후 안에 Enter가 들어오면 Hold로 처리하도록 함.
    /// </summary>
    public class TouchEventHoldingHandler : TouchEventHandler
    {
        const float HOLD_TIME_SEC = 0.08f; // TODO: 실제 장비에서 테스트 후, 근사치 값을 적용해야 함.

        bool Pressed
        {
            get;
            set;
        } = false;


        float pressedTime = 0;
        Vector3 pressPos;

        private void OnEnable()
        {
            Pressed = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void TouchPressed(Vector3 pos)
        {
            pressedTime = Time.time;
            pressPos = pos;
            if (Pressed is false)
            {
                Pressed = true;
                StartCoroutine(TouchRoutine());
            }
        }

        public override void TouchUpdated(Vector3 pos)
        {
            TouchPressed(pos);
        }

        /// <summary>
        /// Release는 사용 안함. 
        /// 자체 루틴으로  Released 계산
        /// </summary>
        public override void TouchReleased(Vector3 pos)
        {
            pressPos = pos;
        }

        public override void Stop()
        {
            base.Stop();

            if (Pressed)
            {
                Pressed = false;
                StopAllCoroutines();
            }
        }


        IEnumerator TouchRoutine()
        {          
            base.TouchPressed(pressPos);

            float oldPressedTime;
            do
            {
                base.TouchUpdated(pressPos);
                oldPressedTime = pressedTime;
                yield return YieldInstructionCache.WaitForSeconds(HOLD_TIME_SEC);
            } while (CheckHold(oldPressedTime, pressedTime));

            Pressed = false;
            base.TouchReleased(pressPos);
        }

        bool CheckHold(float oldTime, float newTime)
        {
            return Mathf.Approximately(oldTime, newTime) is false;
        }
    }

}
