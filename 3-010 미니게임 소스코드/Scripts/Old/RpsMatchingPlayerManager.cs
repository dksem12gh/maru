using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingPlayerManager : MonoBehaviour
    {
        [SerializeField] RpsMatchingPlayer[] players;


        public int ChoiceRange
        {
            get
            {
                if (players.Length < 1) 
                    return 0;

                return players[0].Container.PadCount;
            }
        }

        public RpsMatchingPlayer GetPlayer(PlayerNumber playerNumber)
        {
            foreach (var player in this.players)
            {
                if (player.Number == playerNumber)
                    return player;
            }
            return null;
        }

        public void SetSelecter(IRpsSelecter selecter)
        {
            foreach (var player in this.players)
            {
                player.SetSelecter(selecter);
            }
        }

        public void Pause(bool pause)
        {
            foreach (var player in this.players)
            {
                player.Pause(pause);
            }
        }

        public void Prepare()
        {
            foreach (var player in this.players)
            {
                player.StageEnd();
            }
        }

        public bool IsReady()
        {
            foreach (var player in this.players)
            {
                if (player.IsReady is false)
                    return false;
            }
            return true;
        }

        public bool IsResult()
        {
            if (IsResultFail() || IsResultAnySuccess())
            {
                return true;
            }
            return false;
        }

        public int[] GetResultData()
        {
            int[] resultData = new int[this.players.Length];
            for (int i = 0; i < this.players.Length; i++)
            {
                resultData[i] = this.players[i].ToResultData();
            }
            return resultData;
        }


        public void StageReady()
        {
            foreach (var player in this.players)
            {
                player.StageReady();
            }
        }

        public void StageStart()
        {
            foreach (var player in this.players)
            {
                player.StageStart();
            }
        }
        

        public void StageEnd()
        {
            foreach (var player in this.players)
            {
                player.StageEnd();
            }
        }

        private void SetActiveAllPlayers(bool active)
        {
            foreach (var player in this.players)
            {
                player.SetActive(active);
            }
        }

        private bool IsResultFail()
        {
            foreach (var player in this.players)
            {
                if (player.State != RpsMatchingPlayer.STATE_KIND.FAILED)
                    return false;
            }
            return true;
        }

        private bool IsResultAnySuccess()
        {
            foreach (var player in this.players)
            {
                if (player.State == RpsMatchingPlayer.STATE_KIND.SUCCESS)
                    return true;
            }
            return false;
        }

    }
}