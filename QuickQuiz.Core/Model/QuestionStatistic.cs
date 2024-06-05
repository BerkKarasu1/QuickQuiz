using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public class QuestionStatistic
    {
        public int Id { get; set; }
        public required Question Question { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int InCorrectAnswerCount { get; set; }
    }
}
