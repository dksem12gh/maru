using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace DigitalMaru.MiniGame.MusicMovement
{
    public class EndView : MonoBehaviour
    {
        [Header("Player1")]
        [SerializeField] TMP_Text score0;
        [SerializeField] TMP_Text combo0;
        [SerializeField] TMP_Text perfect0;
        [SerializeField] TMP_Text good0;
        [SerializeField] TMP_Text miss0;
        List<TMP_Text> Player1List = new();

        [Header("Player2")]
        [SerializeField] TMP_Text score1;
        [SerializeField] TMP_Text combo1;
        [SerializeField] TMP_Text perfect1;
        [SerializeField] TMP_Text good1;
        [SerializeField] TMP_Text miss1;
        List<TMP_Text> Player2List = new();

        [Header("Setting")]
        [SerializeField] Image titleImage;          // 노래 표지 이미지
        [SerializeField] TMP_Text musicTitleText;   // 노래 표지 텍스트
        [SerializeField] TMP_Text artistText;       // 작곡가 텍스트
        [SerializeField] Image[] difficulties;      // 난이도(☆☆☆☆☆)

        [Header("ETC")]
        [SerializeField] ParticleSystem particle;
        [SerializeField] GameObject rank;

        List<List<TMP_Text>> PlayerLists = new();

        private void Awake()
        {
            Player1List.Add(score0);
            Player1List.Add(combo0);
            Player1List.Add(perfect0);
            Player1List.Add(good0);
            Player1List.Add(miss0);

            Player2List.Add(score1);
            Player2List.Add(combo1);
            Player2List.Add(perfect1);
            Player2List.Add(good1);
            Player2List.Add(miss1);

            PlayerLists.Add(Player1List);
            PlayerLists.Add(Player2List);
        }


        public List<TMP_Text> GetEndViewList(int playerNum)
        {
            return PlayerLists[playerNum];
        }

        public ParticleSystem GetParticle()
        {
            return particle;
        }

        public GameObject GetRank()
        {
            return rank;
        }


        public void SetEndView(Sprite titleSprite, string musicName, string artistName, Sprite starSprtie)
        {
            titleImage.sprite = titleSprite;
            musicTitleText.text = musicName;
            artistText.text = artistName;

            int musicLevel = Managers.SelGameSet.selectMusic._level;

            for (int i = 0; i < musicLevel; i++)
            {
                difficulties[i].sprite = starSprtie;
            }
        }
    }
}