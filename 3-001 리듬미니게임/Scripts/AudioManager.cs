using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] AudioSource audioSource;       // 메인 오디오 소스
        [SerializeField] List<AudioClip> audioClips;    // 재생할 음악 목록

        [Header("Count Down")]
        [SerializeField] AudioSource soundCount;        // 카운트다운 용 사운드
        [SerializeField] TMP_Text countText;            // 카운트다운 텍스트

        [SerializeField] AudioSource[] _sfxSources; //효과음들
        [SerializeField] AudioSource[] _sfxSourcesFreeMode; //프리모드

        [Header("보이스")]
        [SerializeField] AudioSource[] _voiceSources; //보이스관련

        int musicNum;

        bool _isCount = false;

        // 게임매니저의 Init에서 호출
        public void Init(RhythmGameSettings settings)
        {
            // 곡 설정
            musicNum = settings.MusicNum;
            audioSource.clip = audioClips[musicNum];

            // 오디오 상태 체크 (게임 종료 체크)
            StartCoroutine(nameof(CheckAudioState));
        }

        public void OnePlayerCountSet()
        {
            Vector2 postion = countText.transform.localPosition;
            postion.y += 200;
            countText.transform.localPosition = postion;
        }
        // 3초 카운트 다운
        public IEnumerator ThreeCount()
        {            
            AudioSource countaudio = soundCount;

            for (int i = 3; i > 0; i--)
            {
                countaudio.Play();
                countText.text = i.ToString();
                yield return YieldInstructionCache.WaitForSeconds(1);                
            }

            countText.text = "";            
        }

        // 현재 오디오 클립 반환
        public AudioClip GetCurrentClip()
        {
            return audioClips[musicNum];
        }

        // 메인 오디오 소스 반환
        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        // 게임 종료 체크
        private IEnumerator CheckAudioState()
        {
            while (true)
            {
                // 게임 상태가 Play && 오디오 소스가 오디오 클립의 재생길이만큼 재생되었다면
                if (Managers.SelGameSet.selectGameState == SelectGameState.Play && audioSource.time >= audioSource.clip.length)
                {
                    // 게임 상태를 Finish로 설정
                    Managers.SelGameSet.selectGameState = SelectGameState.Finish;
                }                
                yield return null;
            }
        }
        public void SetBgmVolum(float volume)
        {
            audioSource.volume = volume;
        }
        public void SetSfxVolume(float volume)
        {            
            soundCount.volume = volume;

            foreach (var sfx in _sfxSources)
            {
                if (sfx != null)
                {
                    sfx.volume = volume;
                }
            }
        }
        public void SetVolum_FreeMode(float volume)
        {
            foreach (var sfx in _sfxSourcesFreeMode)
            {
                if (sfx != null)
                {
                    sfx.volume = volume;
                }
            }
        }

        public void SetVoiceVolume(float volume)
        {
            foreach (var voice in _voiceSources)
            {
                if (voice != null)
                {
                    voice.volume = volume;
                }
            }
        }
    }
}