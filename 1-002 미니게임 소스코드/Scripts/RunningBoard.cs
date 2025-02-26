using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Left_and_right_jumping
{
    public class RunningBoard : MonoBehaviour
    {
        [Header("정보")]
        [SerializeField] TMP_Text _info;
        [Header("시간")]
        [SerializeField] TMP_Text _time;
        [Header("횟수")]
        [SerializeField] TMP_Text _count;
        [SerializeField] Animation _countAni;

        public int _gameCurCount;


        #region Info
        public void ClearInfo()
        {
            _info.text = "";
        }
        #endregion


        #region Timer
        public void SetTimer(DigitalMaru.PlayMode playMode, int sec)
        {
            if (!_time) return;

            if (playMode == DigitalMaru.PlayMode.Infinity)
            {
                _time.text = "00:00:00";
            }
            else
            {                
                string timeText = string.Format("{0:00}:{1:00}:{2:00}", 0, (sec / 60), "00");
                _time.text = timeText;
            }
        }

        public void UpdateTimer(int sec)
        {
            if (!_time) return;

            int checkSec = sec;
            if (sec < 0)
            {
                checkSec++;
                checkSec = Mathf.Abs(checkSec);
            }

            int hour = Mathf.FloorToInt(checkSec / 3600);
            int minutes = Mathf.FloorToInt(checkSec / 60 % 60);
            int seconds = Mathf.FloorToInt(checkSec % 60);

            string timeText = string.Format("{0:00}:{1:00}:{2:00}", hour, minutes, seconds);
            _time.text = timeText;
        }
        #endregion


        #region Count
        public void SetCount(int count)
        { 
            _count.text = count.ToString();
        }

        public void PlayCountAni()
        {
            _countAni.Play();
        }

        public void SetGameCountSet(int maxNum)
        {
            _gameCurCount = maxNum;
            _count.text = _gameCurCount.ToString();
        }
        public void SetCurCount(int count,int mode)
        {
            if (mode == -1)
            {
                _gameCurCount = count + 1;
            }
            else
            {
                _gameCurCount = count - 1;
            }
            _count.text = _gameCurCount.ToString();            
        }
        public int GetCurCount()
        {
            return _gameCurCount;
        }
        #endregion
    }
}