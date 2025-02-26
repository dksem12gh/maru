using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public enum ShapeType
    {
        Rock,
        Paper,
        Sissors,
        None
    }

    public enum RuleType
    {
        Win,
        Draw,
        Lose,
        /*
         * UnWin
         * UnDraw
         * UnLose
         */
        None
    }

    public enum ResultType
    {
        Win1P,
        Win2P,
        Draw
    }

    public class RpsGameSettings
    {
        private readonly int QuizMax = Managers.GameTime;

        private readonly int ChoiceCount = 3;

        public int GetMaxQuizCount() => QuizMax;
        public int GetChoiceCount() => ChoiceCount;
    }

    internal class SpriteIndex 
    {
        public const int Normal = 0;
        public const int Pressed = 1;
        public const int Success = 2;
        public const int Failed = 3;
        public const int Disabled = 4;
    }

    internal class SoundIndex
    {
        public const int Click = 0;
        public const int Question = 1;
        public const int Success = 2;
        public const int Failure = 3;
        public const int GameEnd = 4;
    }
}