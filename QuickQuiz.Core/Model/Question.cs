using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string Quest { get; set; }
        public ICollection<Answer> Answers { get; set; } 
        public string TrueAnswer { get; set; }
        public AppUser Creater { get; set; }
    }
    public class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public string AnswerText { get; set; }
    }
}
