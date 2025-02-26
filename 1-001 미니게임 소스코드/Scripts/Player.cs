using System;

namespace DigitalMaru.Exercise.Walk
{
    public class Player
    {
        public int Index = 0;// 0번, 1번 선수

        /// <summary>
        /// Index와 RepCount를 순서대로 보냅니다.
        /// </summary>
        public Action<int, int> RepCountEvent;


        public int RepCount { get; private set; } = 0;


        public void SetRepCount(int value)
        {
            RepCount = value;
            RepCountEvent?.Invoke(Index, RepCount);
        }

        public void IncreaseRepCount()
        {
            SetRepCount(RepCount + 1);
        }

        public (int Count, bool isAni) GetCompletedCycles()
        {
            return (RepCount, true);
        }




    }
}
