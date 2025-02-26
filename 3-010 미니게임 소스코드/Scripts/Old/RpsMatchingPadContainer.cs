using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingPadContainer : MonoBehaviour
    {        
        [SerializeField] List<RpsMatchingPad> choicePadList;

        public Action<int> TouchEvent;

        public int PadCount => choicePadList.Count;


        private void OnEnable()
        {
            foreach (var pad in choicePadList) 
            {
                pad.PressedEvent += Touch;
            }
        }

        private void OnDisable()
        {            
            foreach (var pad in choicePadList) 
            {
                pad.PressedEvent -= Touch;
            }
        }

        public void Touch(int value)
        {
            TouchEvent.Invoke(value);
        }

        public void DisableTouch()
        {
            foreach (var pad in choicePadList) 
            {
                pad.DisableBtn();
            }
        }

        public void EnableTouch()
        {          
            foreach (var pad in choicePadList) 
            {
                pad.EnableBtn();
            }
        } 
    }
}