using UnityEngine;


namespace DigitalMaru.Exercise.Walk
{
    public class GameStage : MonoBehaviour
    {
        [SerializeField] GameManager _owner;
        [SerializeField] GameStagePlayer[] _players;

        public void Init()
        {
            foreach (var player in _players)
            {
                player.Init();
            }
        }

        public void StartStage()
        {
            foreach (var player in _players)
            {
                player.Play();
            }
        }

        public void StopStage()
        {
            foreach (var player in _players)
            {
                player.SetLock(true);
            }
        }

        public void SetLock(bool value)
        {
            foreach (var player in _players)
            {
                player.SetLock(value);
            }
        }


        public void CompletedRep(int playerIndex)
        {
            _owner.CompletedRep(playerIndex);
        }

        public void OnPause(bool pause)
        {
            foreach (var player in _players)
                player.OnPause(pause);
        }
    }
}
