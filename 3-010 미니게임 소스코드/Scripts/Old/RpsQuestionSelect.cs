using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsQuestionSelect : MonoBehaviour
    {
        [SerializeField] Animator RPSShapeAnim;
        [SerializeField] Image[] RPSShapeImg;
        [SerializeField] Animator RPSRuleAnim;
        [SerializeField] Image[] RPSRuleImg;

        [SerializeField] SpriteRenderer[] RpsSprite;

        public bool RpsChcek
        {
            get;
            private set;
        }

        public void RpsQuestionsSelected()
        {
            
        }

        IEnumerator SelectRps()
        {
            return null;
        }
    }
}
