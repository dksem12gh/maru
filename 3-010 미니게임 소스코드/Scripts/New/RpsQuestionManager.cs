using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMaru.MiniGame.RockPaperScissors
{
    public class Answer
    {
        public ShapeType shape;

        public RuleType rule;

        public Answer(ShapeType shape, RuleType rule)
        {
            this.shape = shape;
            this.rule = rule;
        }

        public bool CheckAnswer(ShapeType shape)
        {
            return rule == GetPlayerResult(shape);
        }

        private RuleType GetPlayerResult(ShapeType shape)
        {
            if (this.shape == shape)
                return RuleType.Draw;

            switch(shape)
            {
                case ShapeType.Paper:
                    if(this.shape == ShapeType.Rock)
                        goto default;
                    goto case ShapeType.None;

                case ShapeType.Rock:
                    if (this.shape == ShapeType.Sissors)
                        goto default;
                    goto case ShapeType.None;

                case ShapeType.Sissors:
                    if (this.shape == ShapeType.Paper)
                        goto default;
                    goto case ShapeType.None;

                case ShapeType.None:
                    return RuleType.Lose;

                default:
                    return RuleType.Win;


            }
        }
    }
    /*
     * 3.4.5
     */
    public class Question
    {
        private List<ShapeType> shapeTypes;
        private List<RuleType> rules;

        public Question CreateRandomQuiz(int MaxQuizCount)
        {
            shapeTypes = GenerateRandomShapeList(MaxQuizCount);
            rules = GenerateRandomRuleList(MaxQuizCount);

            return this;
        }

        public List<ShapeType> GetShapeTypes() => shapeTypes;
        public List<RuleType> GetRules() => rules;

        private List<ShapeType> GenerateRandomShapeList(int MaxQuizCount)
        {
            List<ShapeType> temp = new List<ShapeType>();

            for(int i = 0; i < MaxQuizCount; i++)
            {
                temp.Add((ShapeType)Random.Range(0, (int)ShapeType.None));
            }

            return temp;
        }

        private List<RuleType> GenerateRandomRuleList(int MaxQuizCount)
        {
            List<RuleType> temp = new List<RuleType>();

            for(int i = 0; i < MaxQuizCount; i++)
            {
                temp.Add((RuleType)Random.Range(0, (int)RuleType.None));
            }

            return temp;
        }

    }

    public class RpsQuestionManager : MonoBehaviour
    {
        [SerializeField] RpsQuestionDisplay display;

        Question question = new Question();

        public void PrepareUI()
        {
            display.PrepareUI();
        }

        public void CreateRandomList(int MaxQuizCount)
        {
            question = question.CreateRandomQuiz(MaxQuizCount);
        }

        public Answer GenerateQuestion(int index)
        {
            return new Answer(question.GetShapeTypes()[index], question.GetRules()[index]);
        }

        public IEnumerator ShowQuestionAnimation(int index,int level)
        {
            yield return display.ShowQuestion(question, index,level);
        }

        public void EndQuestion()
        {
            display.DisableUI();
        }
    }
}