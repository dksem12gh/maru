using DG.Tweening;
using UnityEngine;


namespace DigitalMaru.Exercise.Walk
{
    public class GamePad : MonoBehaviour
    {
        [Header("소유자")]
        [SerializeField] GameStagePlayer owner;
        [Header("터치")]
        [SerializeField] TouchEventHandler _touchEventHandler;
        [Header("버튼")]
        [SerializeField] GameObject _sprite;
        [SerializeField] SpriteRenderer _spriteRender;
        //[SerializeField] Animator _spriteAni;
        [Header("스프라이트")]
        [SerializeField] Sprite _spriteNormal;
        [SerializeField] Sprite _spritePressed;

        [SerializeField] GameObject _arrow = null;
        //[SerializeField] Sprite _spriteDisabled;
        [Header("FX")]
        [SerializeField] ParticleSystem _fx;
        [Header("사운드")]
        [SerializeField] AudioSource _sndClick;


        public void SetLock(bool value)
        {
            _touchEventHandler.SetLock(value);
        }

        public void Clear()
        {
            _spriteRender.sprite = _spriteNormal;
            _spriteRender.enabled = false;
            _touchEventHandler.SetLock(true);

            _fx.Stop();
            _sndClick.Stop();
            _arrow.SetActive(false);
        }

        public void Init()
        {
            _spriteRender.sprite = _spriteNormal;
            _spriteRender.enabled = false;
            _touchEventHandler.SetLock(true);
            _fx.gameObject.SetActive(false);

            _sndClick.Stop();
            _arrow.SetActive(false);
        }

        public void Ready()
        {
            _spriteRender.sprite = _spriteNormal;
            _spriteRender.enabled = true;
            _touchEventHandler.SetLock(false);

            _arrow.SetActive(true);
        }

        public void Touched()
        {
            owner.Touched(this);

            _spriteRender.sprite = _spritePressed;
            _touchEventHandler.SetLock(true);

            _fx.gameObject.SetActive(true);
            _fx.Play();
            _sndClick.Play();
            _arrow.SetActive(false);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                _spriteRender.enabled = false;
                _spriteRender.sprite = _spriteNormal;
            });
        }

        public void Pause(bool pause)
        {
            _touchEventHandler.SetLock(pause);

            if (pause)
                _arrow.GetComponent<Animation>().Stop();
            else
                _arrow.GetComponent<Animation>().Play();
        }
    }
}
