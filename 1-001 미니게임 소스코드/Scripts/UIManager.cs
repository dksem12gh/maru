using UnityEngine;


namespace DigitalMaru.Exercise.Walk
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] UIPlayer[] _players;
        [SerializeField] GameObject[] _roots;

        private void Start()
        {
            foreach (var root in _roots)
            {
                root.SetActive(false);
            }
        }

        public void Show()
        {
            foreach (var root in _roots)
            {
                root.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (var root in _roots)
            {
                root.SetActive(false);
            }
        }

        public void SetRepCount(int playerIndex, int count)
        {
            _players[playerIndex].Count = count;
        }
    }
}
