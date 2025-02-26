using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace Left_and_right_jumping
{

    public class RunningPlayer : MonoBehaviour
    {
        [SerializeField] RunningBoard _board;
        [Space]
        [SerializeField] RunningPad _p0;
        [SerializeField] RunningPad _p1;

        public RunningBoard GetBoard() { return _board; }
        public RunningPad GetPad0() { return _p0; }
        public RunningPad GetPad1() { return _p1; }


        #region Board
        public void ClearInfo()
        {
            _board.ClearInfo();
        }

        public void SetTimer(DigitalMaru.PlayMode playMode, int sec)
        {
            _board.SetTimer(playMode, sec);
        }

        public void UpdateTimer(int count)
        {
            _board.UpdateTimer(count);
        }

        public void CicleCheckBtnAni(bool isAni)
        {
            //_board.SetCount(count);
            if (isAni)
            {
                _board.PlayCountAni();
            }
        }

        public void SetCountInit(int count)
        {
            _board.SetGameCountSet(count);                        
        }

        public void SetCurCount(int count , int mode)
        {
            _board.SetCurCount(count,mode);
        }

        public int GetCurCount()
        {
            return _board.GetCurCount();
        }
        #endregion


        #region Pad
        public RunningPad GetPadOrNull(DigitalMaru.JUMP2BTN kind)
        {
            if (_p0.Kind == kind) return _p0;
            if (_p1.Kind == kind) return _p1;            

            return null;
        }

        /// <summary>
        /// 현재 패드를 플레이하고 다음 패드를 준비합니다.
        /// </summary>
        /// <param name="padIndex"></param>
        public void PlayCurrentAndReadyNextPad(int padIndex)
        {
            (RunningPad cur, RunningPad next) = padIndex == 0 ? (_p0, _p1) : (_p1, _p0);

            //cur.LightObj.SetActive(false);
            cur.TouchObj.SetActive(false);
            /*cur.NormalObj.SetActive(true);
            cur.NormalAni.Play();*/
            cur.NormalImage.sprite = cur.NSprite[2];
            cur.NormalFX.gameObject.SetActive(true);
            cur.NormalFX.Play();

            next.TouchObj.SetActive(true);
            next.NormalImage.sprite = cur.NSprite[0];
        }

        public void DisactivePad(int padIndex)
        {
            RunningPad padCur = padIndex == 0 ? _p0 : _p1;

            padCur.NormalImage.sprite = padCur.NSprite[1];
            padCur.TouchObj.SetActive(true);

            //padCur.NormalObj.SetActive(false);
            //padCur.LightObj.SetActive(true);
        }
         
        public void ActivePad(int padIndex)
        {
            RunningPad pad = padIndex == 0 ? _p0 : _p1;
            pad.NormalImage.sprite = pad.NSprite[2];
            pad.TouchObj.SetActive(false);
            pad.NormalFX.gameObject.SetActive(false);
            //pad.NormalObj.SetActive(true);
            //pad.LightObj.SetActive(false);
        }

/*        public void ActiveMiddle()
        {
            _pM.NormalObj.SetActive(true);
            _pM.NormalFX.Play();
            _pM.LightImg.enabled = false;
        }

        public void DisactiveMiddle()
        {
            _pM.NormalObj.SetActive(false);
            _pM.LightImg.enabled = true;
        }*/
        #endregion
    }
}