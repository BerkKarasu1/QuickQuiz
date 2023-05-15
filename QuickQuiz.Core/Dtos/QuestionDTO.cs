using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Dtos
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; } = new List<string>();
        public string TrueAnswer { get; set; }
        public AppUser Creater { get; set; }
    }
}
