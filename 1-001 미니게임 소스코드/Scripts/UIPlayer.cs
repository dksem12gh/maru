using TMPro;
using UnityEngine;

namespace DigitalMaru.Exercise.Walk
{
    public class UIPlayer : MonoBehaviour
    {
        //[SerializeField] TMP_Text _info;
        [SerializeField] TMP_Text _count;
        //[SerializeField] Animation _countAni;


        public void HideInfo()
        {
            //_info.gameObject.SetActive(false);
        }

        public int Count
        {
            set
            {
                _count.text = value.ToString();
            }
        }

        public void PlayCountAni()
        {
            /*            if (_countAni != null)
                        {
                            _countAni.Play();
                        }
                        else
                        {
                            Debug.LogWarning("Lie_down_and_change_feet > BoardWindowPlayer > PlayCountAni");
                        }*/
        }
    }
}
