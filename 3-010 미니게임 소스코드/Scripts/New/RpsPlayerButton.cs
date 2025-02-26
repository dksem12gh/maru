using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsPlayerButton : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] SphereCollider colider;
        [SerializeField] Image sprite;

        [Header("Sprites")]
        [SerializeField] List<Sprite> sprites;

        [Header("GameObject")]
        [SerializeField] GameObject Oobj;
        [SerializeField] GameObject Xobj;

        [Header("Values")]
        [SerializeField] ShapeType type = ShapeType.None;

        public bool IsPressed
        {
            get;
            private set;
        }

        public ShapeType GetShape() => type;

        public void Pressed()
        {
            RpsSoundManager.instance.Play(SoundIndex.Click);
            SpriteChange(SpriteIndex.Pressed);
        }

        public void SpriteChange(int index)
        {
            sprite.sprite = sprites[index];

            if(SpriteIndex.Disabled == index)
            {
                Oobj.SetActive(false);
                Xobj.SetActive(false);
            }
        }

        public void Success()
        {
            sprite.sprite = sprites[SpriteIndex.Success];
            Oobj.SetActive(true);
        }

        public void Failed()
        {
            sprite.sprite = sprites[SpriteIndex.Failed];
            Xobj.SetActive(true);
        }

        public void ActivateInput(bool activate)
        {
            colider.enabled = activate;
        }
    }
}