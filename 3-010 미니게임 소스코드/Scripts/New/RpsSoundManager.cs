using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsSoundManager : MonoBehaviour
    {
        [SerializeField] List<AudioSource> sources;

        public static RpsSoundManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void Play(int index)
        {
            sources[index].Play();
        }
    }
}