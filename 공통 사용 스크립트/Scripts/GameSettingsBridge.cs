namespace DigitalMaru
{

    public class GameSettingsBridge
    {
        public int GameCountGoal => Managers.GameTime;

        /// <summary>
        /// 시간을 이용하는 컨텐츠는 60을 곱해줘서, 분을 초로 바꿔준다.
        /// </summary>
        public float GameTimeSecGoal => Managers.GameTime * 60f;
                
        public bool Infinite => Managers.GameTime < 0;

        public SelectGameState CurrentGameState
        {
            get
            {
                if (Managers.SelGameSet != null) return Managers.SelGameSet.selectGameState;
                return SelectGameState.Play;
            }
        }
    }
}
