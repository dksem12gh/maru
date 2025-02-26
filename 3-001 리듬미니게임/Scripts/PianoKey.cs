using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class PianoKey : MonoBehaviour
    {
        [SerializeField] GameObject originImage;    // Normal 이미지
        [SerializeField] GameObject pressInput;     // Press 이미지
        [SerializeField] GameObject pressColor;     // Press 빛(이펙트) 이미지
        [SerializeField] ParticleSystem particle;   // Press Particle 
        [SerializeField] Animation anim;
        float raycastLength;

        JudgementManager judgementManager;
        AudioSource audioSource;

        BoxCollider boxCollider;

        Coroutine _HoldCoroutine;

        // Player Init에서 호출
        public void Init(JudgementManager _judgementManager)
        {
            judgementManager = _judgementManager;

            particle.gameObject.SetActive(false);
            audioSource = GetComponent<AudioSource>();
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        // 노트 판정 이펙트
        public void NoteCheckEffect()
        {
            particle.gameObject.SetActive(true);
            particle.Play();

            anim.gameObject.SetActive(true);
            anim.Stop();
            anim.Play();

            audioSource.Play();
        }

        // 건반 터치 이펙트
        public void TouchEffect()
        {
            StartCoroutine(nameof(TouchBlinkEffect));
        }
        IEnumerator TouchBlinkEffect()
        {
            pressInput.SetActive(true);
            originImage.SetActive(false);
            pressColor.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            originImage.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            pressColor.SetActive(false);
            pressInput.SetActive(false);
        }

        public void TouchEnable(bool enable)
        {
            boxCollider.enabled = enable;
        }

        public void AutoPressEvent()
        {
            NoteCheckEffect();
            TouchEffect();
        }

        //홀드용
        public void PressEvent()
        {
            if(_HoldCoroutine != null)
                StopCoroutine(_HoldCoroutine);

            pressInput.SetActive(true);
            originImage.SetActive(false);
            pressColor.SetActive(true);

            _HoldCoroutine = StartCoroutine(Hide());
        }
        private IEnumerator Hide()
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            originImage.SetActive(true);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            pressColor.SetActive(false);
            pressInput.SetActive(false);
        }

        // 건반 터치 이벤트
        public void KeyPressEvent()
        {
            //TouchEffect();
            PressEvent();

            // 레이캐스트 속성 값 설정
            Vector3 keyPosition = transform.position;
            Vector3 direction = Vector3.up;
            Bounds judgementBound = judgementManager.judgementBound;

            // 레이캐스트 길이 보정
            raycastLength = judgementBound.max.y - transform.position.y - 0.5f;
            int layerMask = 1 << LayerMask.NameToLayer("Note");
           
            // 레이캐스트
            RaycastHit hit;
            if (Physics.Raycast(keyPosition, direction, out hit, raycastLength, layerMask))
            {
                if (hit.collider.CompareTag("Note"))
                {
                    judgementManager.CheckNote(hit.transform.GetComponent<Note>());

                    NoteCheckEffect();
                }

                Debug.DrawRay(keyPosition, direction * raycastLength, Color.red, 1.0f);
            }

        }
    }
}