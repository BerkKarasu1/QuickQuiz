using Mapster;
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
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public Answer? TrueAnswer { get; set; }
        public List<TestDTO>? Tests { get; set; } = new List<TestDTO>();
        public AppUser Creater { get; set; }
        public bool Check { get; set; }
        public string? TestName { get; set; }

    }
}
