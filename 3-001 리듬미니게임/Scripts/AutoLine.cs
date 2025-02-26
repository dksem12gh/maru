using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalMaru.MiniGame.MusicMovement
{
    // 자동 연주 모드에서 노트를 인식, 처리할 라인
    public class AutoLine : MonoBehaviour
    {
        [SerializeField] JudgementManager judgementManager;     // 판정 매니저

        private void OnTriggerEnter(Collider other)
        {
            // 노트가 감지 되었다면
            if (other.CompareTag("Note"))
            {
                // 판정 매니저로 탐지한 노트를 넘김
                judgementManager.CheckAutoNote(other.GetComponent<Note>());
            }
        }
    }
}