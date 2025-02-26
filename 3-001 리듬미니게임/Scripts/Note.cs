using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Note : MonoBehaviour
    {
        int line;
        public int Line
        {
            get { return line; }
            set
            {
                line = value;
                GetComponent<Image>().sprite = line == 0 || line == 3 ? purpleSprite : greedSprite;
            }
        }

        public int sequentialOrder;

        [SerializeField] Sprite purpleSprite;
        [SerializeField] Sprite greedSprite;
        [SerializeField] BoxCollider boxCollider;

        public void TouchEnable(bool enable)
        {
            boxCollider.enabled = enable;
        }
    }
}