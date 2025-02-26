using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalMaru
{
    public enum SndKind
    {
        CountDown,
        Click,
        TimeTo,
        End,
        Success,
        Fail
    }

    public class SubSoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource soundCountDown = null;
        [SerializeField] AudioSource soundClick = null;
        [SerializeField] AudioSource soundTimeTo = null;
        [SerializeField] AudioSource soundEnd = null;
        [SerializeField] AudioSource soundSuccess = null;
        [SerializeField] AudioSource soundFaile = null;


        public void Play(SndKind kind)
        {
            switch (kind)
            {
                case SndKind.CountDown:
                    soundCountDown.Play();
                    break;
                case SndKind.Click:
                    soundClick.Play();
                    break;
                case SndKind.TimeTo:
                    soundTimeTo.Play();
                    break;
                case SndKind.End:
                    soundEnd.Play();
                    break;
                case SndKind.Success:
                    soundSuccess.Play();
                    break;
                case SndKind.Fail:
                    soundSuccess.Play();
                    break;
            }
        }

    }
}