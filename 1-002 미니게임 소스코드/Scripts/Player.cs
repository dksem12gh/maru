using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Left_and_right_jumping
{
    public enum PlayerState
    {
        None,
        Ready,
        Run,
        Complete,
    }
    public class Player
    {
        public int Index = 0;
        public PlayerState State = PlayerState.None;

        public int LeftTimeSec = 0;
        public int AllTimeSec = 0;
        public float StartTime = 0;
        public int RunningCount = 0;
        public int GameMaxCount = 0;

        public void SetGameMaxCount(int num)
        {
            GameMaxCount = num;
        }

        public int GetPadIndex()
        {
            return RunningCount % 2;
        }

        public (int Count, bool isAni) GetCompletedCycles()
        {
            return (RunningCount / 2, RunningCount % 2 == 0);
        }

        public DigitalMaru.ResultData ToResultData()
        {
            return new DigitalMaru.ResultData()
            {
                CyclesCount = this.RunningCount / 2,
            };            
        }
    }
}
