using UnityEngine;
using UnityEngine.UI;

namespace Left_and_right_jumping
{
    public class RunningPad : MonoBehaviour
    {
        public DigitalMaru.JUMP2BTN Kind;
        [Space]
        public GameObject TouchObj;        
        public TouchEventHandler EventTouch;
        [Space]
        public GameObject NormalObj;
        public Animation NormalAni;
        public ParticleSystem NormalFX;
        public SpriteRenderer NormalImage;
        public Sprite[] NSprite;
    }
}