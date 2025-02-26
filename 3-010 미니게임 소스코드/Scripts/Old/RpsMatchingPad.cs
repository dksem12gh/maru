using System;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingPad : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TouchEventHoldingHandler touchHandler;
        [SerializeField] Animation padAnimation;
        [Header("Variable")]
        [SerializeField] GameObject hideObj;
        [SerializeField] int value = 0;

        public Action<int> PressedEvent;
        public Action ReleasedEvent;

        private void OnEnable()
        {
            touchHandler.m_PressedEvent.AddListener(PressedTouch);
            touchHandler.m_ReleasedEvent.AddListener(ReleasedTouch);
        }

        private void OnDisable()
        {
            touchHandler.m_PressedEvent.RemoveListener(PressedTouch);
            touchHandler.m_ReleasedEvent.RemoveListener(ReleasedTouch);
        }

        public void ReleasedTouch(Vector3 vector3)
        {
            ReleasedEvent?.Invoke();
        }

        public void PressedTouch(Vector3 vector3)
        {
            if (touchHandler.enabled == false)
                return;

            if (padAnimation != null)
            {
                padAnimation?.Play();
            }

            PressedEvent.Invoke(this.value);
        }

        public void DisableBtn()
        {
            touchHandler.enabled = false;
            hideObj.SetActive(true);
        }

        public void EnableBtn()
        {
            touchHandler.enabled = true;
            hideObj.SetActive(false);
        }
    }
}