using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru
{

    static public class Helper
    {
        /// <summary>
        /// 초를 시,분,초로 표시
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        static public string FormatToHMSFromSec(int sec) //시간 표기 변환
        {
            int hours = Mathf.FloorToInt(sec / 3600);
            int minutes = Mathf.FloorToInt((sec / 60 ) % 60);
            int seconds = Mathf.FloorToInt(sec % 60);

            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }

        static public string FormatToHMSFromScore(int sec) //시간 표기 변환
        {
            int seconds = Mathf.FloorToInt(sec % 60);
            return string.Format("{0:D1}",seconds);
        }        

        /// <summary>
        /// 분을 시,분,초 표시
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        //static public string FormatToHMSFromMin(int min) 
        //{
        //    int hours = min / 60;
        //    int minutes = min % / 60;

        //    return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, 0);
        //}


        static public void StopCoroutine(MonoBehaviour mono, Coroutine _coroutine)
        {
            if (_coroutine != null)
            {
                mono.StopCoroutine(_coroutine);
            }
        }
    }
}