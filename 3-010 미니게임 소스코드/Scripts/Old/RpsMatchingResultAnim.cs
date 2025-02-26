using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingResultAnim : MonoBehaviour
    {
        [SerializeField] Animation successAnim;
        [SerializeField] Animation failAnim;

        public void Stop()
        {
            successAnim.gameObject.SetActive(false);
            failAnim.gameObject.SetActive(false);
        }

        public void PlaySuccess()
        {
            successAnim.gameObject.SetActive(true);
        }

        public void PlayFail()
        {
            failAnim.gameObject.SetActive(true);
        }
    }
}