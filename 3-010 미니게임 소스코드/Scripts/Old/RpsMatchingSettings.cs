
namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class RpsMatchingSettings : GameSettingsBridge
    {
        public int QuizMax { get; private set; } = 3;
        public int ChoiceRps { get; private set; } = 3;
        public int ChoiceRule { get; private set; } = 3;

        public RpsMatchingSettings(int choiceRange)
        {
            this.ChoiceRps = choiceRange;
            this.ChoiceRule = choiceRange;
            this.QuizMax = this.GameCountGoal;
        }
    }
}
