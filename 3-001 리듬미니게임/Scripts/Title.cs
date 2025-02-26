using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Title : MonoBehaviour
    {
        [Header("타이틀")]
        [SerializeField] TMP_Text titleTxt;         // 타이틀 텍스트
        [SerializeField] TMP_Text songNameTxt;      // 음악 텍스트

        public void Init(RhythmGameSettings settings)
        {
            titleTxt.text = settings.ModeName;
            songNameTxt.text = settings.MusicName;
        }
    }
}