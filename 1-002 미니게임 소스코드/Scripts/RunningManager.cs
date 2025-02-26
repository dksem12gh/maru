using UnityEngine;
using UnityEngine.Events;
using DigitalMaru;


namespace Left_and_right_jumping
{

    public class RunningManager : MonoBehaviour
    {
        [SerializeField] RunningPlayer[] _players = new RunningPlayer[2];


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public RunningPlayer GetPlayer(int playerIndex)
        {
            return _players[playerIndex];
        }


        public void SetTimer(DigitalMaru.PlayMode playMode, int sec)
        {
            foreach (var player in _players)
            {
                player.SetTimer(playMode, sec);
            }
        }


        #region LightTouch
        public void LightTouch_AddListener(JUMP2BTN kind, UnityAction<Vector3> call)
        {
            GetPadOrNull(kind).EventTouch.m_PressedEvent.AddListener(call);
        }

        public void LightTouch_RemoveListener(JUMP2BTN kind, UnityAction<Vector3> call)
        {
            GetPadOrNull(kind).EventTouch.m_PressedEvent.RemoveListener(call);
        }

        private RunningPad GetPadOrNull(JUMP2BTN kind)
        {
            foreach (RunningPlayer player in _players)
            {
                RunningPad pad = player.GetPadOrNull(kind);
                if (pad != null)
                {
                    return pad;
                }
            }
            return null;
        }
        #endregion

    }
}