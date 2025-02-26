using System.Collections;
using UnityEngine;


namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Piano_Press : MonoBehaviour
    {
        [SerializeField] AudioSource sound;
        [SerializeField] GameObject objPress;
        [SerializeField] GameObject objPressColor;

        public void PressEvent()
        {
            sound.Play();
        }
        public void UpdateEvent()
        {
            objPress.SetActive(true);
            objPressColor.SetActive(true);
        }
        public void ReleaseEvent()
        {
            objPress.SetActive(false);
            objPressColor.SetActive(false);
        }
    }
}